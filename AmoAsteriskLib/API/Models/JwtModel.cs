namespace AmoAsterisk.ApiManagement;

public class JwtModel {
  public string token_type { get; set; } = "Bearer";
  public int expires_in { get; set; }
  public string access_token { get; set; } = string.Empty;
  public string refresh_token { get; set; } = string.Empty;
}