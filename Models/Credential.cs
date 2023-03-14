namespace ArbisSalesManagers.Models;

#nullable disable warnings
public class Credential : IAdmin {
  public string Username { get; set; }
  public byte[] Password_Salt { get; set; }
  public byte[] Password_Hash { get; set; }
  public bool Is_Admin { get; set; } = false;
}