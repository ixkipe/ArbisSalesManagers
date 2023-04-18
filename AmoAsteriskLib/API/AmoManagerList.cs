using AmoAsterisk.DbAccess;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RestSharp;

namespace AmoAsterisk;

public class AmoManagerList {
  private readonly IConfiguration _config;
  public IEnumerable<AmoCrmUserModel> Managers { get => GetManagers(); }
  public AmoManagerList(IConfiguration config)
  {
    _config = config;
  }

  private IEnumerable<AmoCrmUserModel> GetManagers() {
    // string managerData = File.ReadAllText(LocationMetaData.UsersDataPath);
    // var managers = JsonConvert.DeserializeObject<IEnumerable<AmoCrmUserModel>>(managerData);
    // return managers;
    var url = this._config.GetSection("ApiAccess").GetValue<string>("LocalLink");

    using var client = new RestClient(new RestClientOptions() {
      BaseUrl = new Uri(url)
    });
    
    Log.Information(url);

    try {
      var response = client.Get(new RestRequest(resource: ""));

      if (!response.IsSuccessStatusCode) {
        return this._config.GetValue<IEnumerable<AmoCrmUserModel>>("Managers");
      }

      return JsonConvert.DeserializeObject<IEnumerable<AmoCrmUserModel>>(response.Content);
    }
    catch (HttpRequestException e) {
      Log.Error(e, e.Message);
      return this._config.GetSection("Managers").Get<AmoCrmUserModel[]>();
    }
  }
}