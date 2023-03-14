using System.ComponentModel.DataAnnotations;

namespace ArbisSalesManagers.Models;

public class RegisterDto : IAdmin {
  [Required]
  public string Username { get; set; }

  [Required]
  [DataType(DataType.Password)]
  public string Password { get; set; }
  public bool Is_Admin { get; set; } = false;
}