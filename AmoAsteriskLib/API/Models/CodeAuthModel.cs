using Microsoft.Extensions.Configuration;

namespace AmoAsterisk.ApiManagement;

public class CodeAuthModel : AuthModel{
  public CodeAuthModel(IConfiguration config) : base(config)
  {
  }

  public override string grant_type => "authorization_code";
  public string code { get; set; }
}