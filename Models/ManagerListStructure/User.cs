namespace ArbisSalesManagers.Models;

public class User {
  public string Id { get; set; }
  public string Username { get; set; }
  public string? Num { get; set; }
  public bool is_active { get; set; }
}