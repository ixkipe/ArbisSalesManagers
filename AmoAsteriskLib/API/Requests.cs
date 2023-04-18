using Microsoft.Extensions.Configuration;

namespace AmoAsterisk.ApiManagement;

public class RequestManager {
  private string WebsiteBase;
  private const string ContactsPostfix = "/api/v4/contacts";
  private const string RetrieveTokenPostfix = "/oauth2/access_token";
  private const string UsersPostfix = "/api/v4/users";
  private const string AddCallsPostfix = "/api/v2/calls";
  public string Get_Contacts { get => $"{WebsiteBase}{ContactsPostfix}"; }
  public string Get_Users { get => $"{WebsiteBase}{UsersPostfix}"; }
  public string Post_RetrieveToken { get => $"{WebsiteBase}{RetrieveTokenPostfix}"; }
  public string Post_MultipleCalls { get => $"{WebsiteBase}{AddCallsPostfix}"; }

  public RequestManager(IConfiguration config)
  {
    this.WebsiteBase = config.GetSection("AppConfig").GetValue<string>("AmoCrmBaseUrl");
  }
}