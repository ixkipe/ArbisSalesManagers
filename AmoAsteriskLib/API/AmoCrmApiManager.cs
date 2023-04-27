using AmoAsterisk.DbAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Serilog;

namespace AmoAsterisk.ApiManagement;

public class AmoCrmApiManager {
  private readonly IConfiguration _config;
  private readonly IServiceProvider _services;

  public AmoCrmApiManager(IConfiguration config, IServiceProvider services)
  {
    _config = config;
    _services = services;
  }

  private bool ValidTimeToAddCalls {
    get {
      var currentTime = DateTime.Now;
      if (currentTime.DayOfWeek == DayOfWeek.Sunday || currentTime.DayOfWeek == DayOfWeek.Saturday || currentTime.Hour < 9 || currentTime.Hour > 17)
      return false;
      
      return true;
    }
  }

  public async Task AddCallsAsync(DateTime from) {
    AddCallsPostBodyMaker bodyMaker = new(this._services);
    var body = await bodyMaker.FormJsonRequestBody(from);
    
    if (body.Key is null) {
      Log.Information("No calls found; thus, none added.");
      return;
    }

    await PostCalls(body.Key);
  }

  private async Task<bool> PostCalls(string jsonBody) {
    using (var client = new HttpClient()) {
      client.BaseAddress = new Uri(new RequestManager(this._config).Post_MultipleCalls);
      client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
      );
      client.DefaultRequestHeaders.Add(
        "Authorization", "Bearer " + new AmoCrmSession(string.Empty, _config, this._services.GetService<IMysqlMiscConnectionProvider>()).AccessToken
      );
      
      var response = await client.PostAsync(string.Empty, new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json"));
      if (response.IsSuccessStatusCode) {
        Log.Information("Okay (token still valid). Calls added.");
        return true;
      }
    }

    Log.Warning("The token seems to have expired.");
    using (var client = new HttpClient()) {
      // forgot to add this fecking line
      client.BaseAddress = new Uri(new RequestManager(this._config).Post_MultipleCalls);
      client.DefaultRequestHeaders.Accept.Add(
        new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")
      );
      AmoCrmSession session = new(string.Empty, this._config, this._services.GetService<IMysqlMiscConnectionProvider>());
      await session.RefreshCreds();
      client.DefaultRequestHeaders.Add(
        "Authorization", "Bearer " + session.AccessToken
      );
      var newResponse = await client.PostAsync(string.Empty, new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json"));
      if (newResponse.IsSuccessStatusCode) {
        Log.Warning("Token has been refreshed.");
        Log.Information("Calls added.");
        return true;
      }

      Log.Error("No valid token found; awaiting authorization code.");
      return false;
    }
  }

  public async Task AddCallsContinuously(bool waitAMinute) {
    Log.Information("\nWill begin fetching new calls in 1 minute.");
    if (waitAMinute) await Task.Delay(60000);

    if (!this.ValidTimeToAddCalls) {
      Log.Information("Paused. Invalid time to add calls.");
      return;
    }

    var body = await new AddCallsPostBodyMaker(this._services).FormJsonRequestBody();
    if (body.Key is null) {
      Log.Information("No calls found; thus, none added.");
      return;
    }
    Log.Information(body.Key);
    
    var success = await PostCalls(body.Key);
    if (success) await CallFetcher.UpdateReferenceTime(body.Value, this._services);
  }
}