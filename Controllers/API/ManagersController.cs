using ArbisSalesManagers.AmoCrmApiRequests;
using ArbisSalesManagers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace ArbisSalesManagers.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Policy = "RegularUser")]
public class ManagersController : ControllerBase {
  private readonly IDbConnectionProvider _provider;
  private readonly IMapper _mapper;
  private readonly IConfiguration _configuration;
  private readonly IJwtHandler _jwtHandler;

  public ManagersController(IDbConnectionProvider provider, IMapper mapper, IConfiguration configuration, IJwtHandler jwtHandler)
  {
    _provider = provider;
    _mapper = mapper;
    _configuration = configuration;
    _jwtHandler = jwtHandler;
  }

  [HttpGet]
  public async Task<IActionResult> GetAllManagers() {
    var managers = (await AmoManagerList()).Where(x => x.is_active);

    var managersIds = string.Join(", ", managers.Select(x => x.Id));

    // active - those whose calls are to be logged
    var activeManagers = await this._provider.Connection.QueryAsync<User>(
      $"select * from {MetaData.ManagersTable} where id in ({managersIds})" // is this a vulnerability or what?!
    );

    // inactive - their calls don't get logged, but they still own a number
    var inactiveManagers = await this._provider.Connection.QueryAsync<User>(
      $"select * from {MetaData.InactiveManagersTable} where id in ({managersIds})"
    );

    // managers = unassigned, have no number and are inactive
    managers = managers.Except(activeManagers.Concat(inactiveManagers), new UserEqualityComparer());

    return Ok(
      new {
        count = managers.Count() + activeManagers.Count() + inactiveManagers.Count(),
        active = activeManagers,
        inactive = inactiveManagers,
        unassigned = managers
      }
    );
  }

  [HttpGet("active")]
  [AllowAnonymous]
  public async Task<IActionResult> GetActiveManagers() {
    var activeManagers = await this._provider.Connection.QueryAsync<User>(
      Queries.GetAllManagers
    );

    return Ok(activeManagers);
  }

  // METHODS:
  // 1. assign a phone number to a manager (DONE (kinda))
  // 2. move manager from active to inactive
  // 3. move manager from inactive to active
  // 4. edit phone number of a certain manager

  [HttpPost]
  public async Task<IActionResult> AssignPhoneNum([FromBody] User user) {
    await this._provider.Connection.ExecuteAsync(
      Queries.CreateManager,
      new {
        id = user.Id,
        username = user.Username,
        num = user.Num
      }
    );

    return Ok(new {
      result = $"Менеджер с именем {user.Username} и номером {user.Num} успешно добавлен!"
    });
  }

  [HttpGet("enable/{id}")]
  public async Task<IActionResult> SetActive(string id) => throw new NotImplementedException();

  [HttpGet("disable/{id}")]
  public async Task<IActionResult> SetInactive(string id) => throw new NotImplementedException();

  [HttpPut]
  public async Task<IActionResult> EditNumber([FromBody] User user) => throw new NotImplementedException();

  private async Task<IEnumerable<User>> AmoManagerList() {
    IEnumerable<User> managers = Enumerable.Empty<User>();

    using (var client = new RestClient(new RestClientOptions() {
      ThrowOnAnyError = false,
      BaseUrl = new Uri(this._configuration.GetSection("AppConfig").GetSection("AmoCrmBaseUrl").Value)
    })) 
    {
      client.Authenticator = new JwtAuthenticator(this._jwtHandler.AccessToken);
      var request = new RestRequest(Requests.GetAllManagers);

      var response = await client.ExecuteGetAsync(request);
      Log.Information(response.StatusCode.ToString());
      if (response.StatusCode != System.Net.HttpStatusCode.OK) {
        Log.Error("Access token is either invalid or has expired. Refreshing...");
        this._jwtHandler.Refresh();
        client.Authenticator = new JwtAuthenticator(this._jwtHandler.AccessToken);
        response = await client.ExecuteGetAsync(request);

        if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new HttpRequestException($"Either the refresh token has expired or something else has gone wrong: {response.ErrorException?.ToString()}");
      };
    
      managers = JsonConvert.DeserializeObject<ListResult>(response.Content)._embedded.Users.Select(u => _mapper.Map<User>(u));
    }

    return managers;
  }

  // !!!
  // what follows applies to test data and is to be deleted in the final version

  // [HttpGet("test/all")]
  // [AllowAnonymous]
  // public async Task<IActionResult> GetAllManagersTest(bool inactive = false) {
  //   var managersFromDb = JsonConvert.DeserializeObject<IEnumerable<User>>(System.IO.File.ReadAllText(MetaData.ActiveManagers));
  //   if (!inactive) return Ok(managersFromDb);

  //   var inactiveManagers = JsonConvert.DeserializeObject<IEnumerable<User>>(System.IO.File.ReadAllText(MetaData.InactiveManagers));
  //   var amoManagers = JsonConvert.DeserializeObject<IEnumerable<AmoCrmUser>>(System.IO.File.ReadAllText(MetaData.ManagersFromAmoCrm))
  //                             .Select(x => _mapper.Map<User>(x)).Except(managersFromDb.Concat(inactiveManagers), new UserEqualityComparer());

  //   return Ok(new {
  //     count = new { active = managersFromDb.Count(), inactive = inactiveManagers.Count(), unassigned = amoManagers.Count() }, 
  //     active = managersFromDb, 
  //     inactive = inactiveManagers, 
  //     unassigned = amoManagers
  //   });
  // }

  // [HttpGet("test/amomanagers")]
  // [AllowAnonymous]
  // public async Task<IActionResult> GetAmoManagers() {
  //   using var client = new RestClient(new RestClientOptions() {
  //     ThrowOnAnyError = false,
  //     BaseUrl = new Uri(this._configuration.GetSection("AppConfig").GetSection("AmoCrmBaseUrl").Value)
  //   });
    
  //   // GET JWT FROM DATABASE
  //   client.Authenticator = new JwtAuthenticator(this._jwtHandler.AccessToken);
  //   var request = new RestRequest(Requests.GetAllManagers);

  //   var response = await client.ExecuteGetAsync(request);
  //   Log.Information(response.StatusCode.ToString());
  //   if (response.StatusCode != System.Net.HttpStatusCode.OK) {
  //     Log.Error("Access token is either invalid or has expired. Refreshing...");
  //     this._jwtHandler.Refresh();
  //     client.Authenticator = new JwtAuthenticator(this._jwtHandler.AccessToken);
  //     response = await client.ExecuteGetAsync(request);

  //     if (response.StatusCode != System.Net.HttpStatusCode.OK) throw new HttpRequestException($"Either the refresh token has expired or something else has gone wrong: {response.ErrorMessage}");
  //   };

  //   // we split the managers into three groups, just like in the method above: active, inactive, unassigned

  //   var managers = JsonConvert.DeserializeObject<ListResult>(response.Content)._embedded.Users.Select(u => _mapper.Map<User>(u));

  //   return Ok(new { count = managers.Count(), managers = managers });
  // }
}