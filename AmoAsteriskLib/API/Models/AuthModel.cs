using Microsoft.Extensions.Configuration;

namespace AmoAsterisk.ApiManagement;

// configure whatever creds you have here
public abstract class AuthModel {
  public string client_id;
  public string client_secret;
  public abstract string grant_type { get; }
  public string redirect_uri;

  public AuthModel(IConfiguration config)
  {
    this.client_id = config.GetSection("AppConfig").GetValue<string>("ClientId");
    this.client_secret = config.GetSection("AppConfig").GetValue<string>("ClientSecret");
    this.redirect_uri = config.GetSection("AppConfig").GetValue<string>("RedirectUri");
  }
}