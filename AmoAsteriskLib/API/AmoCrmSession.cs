using System.Text.Json;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Data.Sqlite;
using Dapper;
using AmoAsterisk.DbAccess;
using System.Net.Http.Json;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using Serilog;

namespace AmoAsterisk.ApiManagement;

public class AmoCrmSession {
  private readonly IConfiguration _config;
  private readonly IMysqlMiscConnectionProvider _provider;
  private string _authCode;
  public string? AccessToken { get; set; }

  public AmoCrmSession(string authCode, IConfiguration config, IMysqlMiscConnectionProvider provider)
  {
    this._authCode = authCode;
    _provider = provider;
    _config = config;
    var temp = TokenNotExpired();
    if (temp.Key)
    {
      this.AccessToken = temp.Value;
    }
    else {
      RefreshCreds().Wait();
    }
  }

  public async Task RefreshCreds() {
    var creds = await this._provider.Connection.QueryFirstAsync<SldbCredsModel>(SldbMetaData.FetchCreds);
    
    bool refreshOk = await SendCredsRequestAsync(this._authCode, creds, this._provider.Connection, true);
    if (!refreshOk) {
      refreshOk = await SendCredsRequestAsync(this._authCode, creds, this._provider.Connection, false);
      if (refreshOk) return;

      while (!refreshOk) {
        Log.Warning("Awaiting authorization code...");
        string freshAuthCode = Console.ReadLine() ?? string.Empty;
        refreshOk = await SendCredsRequestAsync(freshAuthCode, creds, this._provider.Connection, false);
      }
    }
  }

  private KeyValuePair<bool, string> TokenNotExpired() {
    using (var client = new HttpClient()) {
      string key = (this._provider.Connection.QueryFirst<SldbCredsModel>(SldbMetaData.FetchCreds)).Access_Token;

      client.BaseAddress = new Uri(new RequestManager(this._config).Get_Contacts);
      client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
      );
      client.DefaultRequestHeaders.Add(
        "Authorization", "Bearer " + key
      );

      var response = client.GetAsync(string.Empty).Result;
      return new(response.IsSuccessStatusCode, key);
    }
  }

  private async Task<bool> SendCredsRequestAsync(string authCode, SldbCredsModel creds, MySqlConnection connection, bool tryRefresh) {
    using (var client = new HttpClient()) {
      client.BaseAddress = new Uri(new RequestManager(this._config).Post_RetrieveToken);
      client.DefaultRequestHeaders.Accept.Add(
        new MediaTypeWithQualityHeaderValue("application/json")
      );
      
      // HttpRequestMessage msg = new();
      // if (tryRefresh) {
      //   msg.Content = JsonContent.Create(new RefreshAuthModel() { refresh_token = creds.Refresh_Token });
      // }
      // else msg.Content = JsonContent.Create(new CodeAuthModel() { code = authCode });

      HttpResponseMessage? response = null;

      if (tryRefresh) {
        response = await client.PostAsJsonAsync(client.BaseAddress, new RefreshAuthModel(this._config) { refresh_token = creds.Refresh_Token });
      }
      else {
        response = await client.PostAsJsonAsync(client.BaseAddress, new CodeAuthModel(this._config) { code = authCode });
      }

      if (response.IsSuccessStatusCode) {
        var newCreds = await response.Content.ReadAsAsync<JwtModel>();
        this.AccessToken = newCreds.access_token;
        
        await connection.ExecuteAsync(
          SldbMetaData.UpdateCreds,
          new {
            accessToken = newCreds.access_token,
            refreshToken = newCreds.refresh_token
          }
        );
        return true;
      }

      return false;
    }
  }
}