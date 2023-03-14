using ArbisSalesManagers.Models;

namespace ArbisSalesManagers.DataAccess;

public interface IUserValidator {
  bool UserExists(CredentialDTO user);
  bool PasswordCorrect(CredentialDTO user);
}