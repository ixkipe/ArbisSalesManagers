using Microsoft.Extensions.Configuration;

namespace AmoAsterisk.ApiManagement;

public class RefreshAuthModel : AuthModel {
  public RefreshAuthModel(IConfiguration config) : base(config)
  {
  }

  public override string grant_type => "refresh_token";
  public string refresh_token { get; set; }
}