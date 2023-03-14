using System.ComponentModel.DataAnnotations;

namespace ArbisSalesManagers.Models;

#nullable disable warnings
public class CredentialDTO : IValidatableObject {
  [Required(ErrorMessage = "Требуется имя пользователя.")]
  [Display(Name = "Имя пользователя")]
  public string Username { get; set; }

  [Required(ErrorMessage = "Требуется пароль.")]
  [DataType(DataType.Password)]
  [Display(Name = "Пароль")]
  public string Password { get; set; }

  public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
  {
    var userValidator = validationContext.GetService(typeof(IUserValidator)) as IUserValidator;
    if (!userValidator.UserExists(this)) {
      yield return new ValidationResult("Пользователь не найден.", new[] { nameof(this.Username) });
    }
    else {
      if (!userValidator.PasswordCorrect(this)) {
        yield return new ValidationResult("Неверный пароль.", new[] { nameof(this.Password) });
      }
    }
  }
}