using RestSharp;
using Newtonsoft.Json;

namespace ArbisSalesManagers.AmoCrmApiRequests;

public class JwtHandler : IJwtHandler {
  private readonly IDbConnectionProvider _provider;
  private readonly IConfiguration _config;

  public JwtHandler(IDbConnectionProvider provider, IConfiguration config)
  {
    _provider = provider;
    _config = config;
  }

  public string AccessToken {
    get => this._provider.Connection.QueryFirst<string>(Queries.FetchAmoAccessToken);
  }

  public void Refresh() {
    using var client = new RestClient(this._config.GetSection("AppConfig").GetSection("AmoCrmBaseUrl").Value);
    
    var refreshToken = this._provider.Connection.QueryFirst<string>(Queries.FetchAmoRefreshToken);
    var request = new RestRequest(Requests.RefreshToken, Method.Post) {
      RequestFormat = DataFormat.Json
    };
    request.AddJsonBody(new {
      client_id = this._config.GetSection("AppConfig").GetSection("ClientId").Value,
      client_secret = this._config.GetSection("AppConfig").GetSection("ClientSecret").Value,
      grant_type = "refresh_token",
      refresh_token = refreshToken,
      redirect_uri = this._config.GetSection("AppConfig").GetSection("RedirectUri").Value,
    });

    var response = client.Post(request);
    if (response.IsSuccessStatusCode) {
      var tokens = JsonConvert.DeserializeObject<Token>(response.Content);

      this._provider.Connection.Execute(
        Queries.UpdateAmoWebCreds,
        new {
          acc = tokens.access_token,
          refr = tokens.refresh_token
        }
      );
    }
  }
}