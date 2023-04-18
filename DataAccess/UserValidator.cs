using System.Security.Cryptography;
using AmoAsterisk.DbAccess;
using ArbisSalesManagers.Models;

namespace ArbisSalesManagers.DataAccess;

public class UserValidator : IUserValidator {
  private readonly IMysqlMiscConnectionProvider _provider;

  public UserValidator(IMysqlMiscConnectionProvider provider)
  {
    _provider = provider;
  }

  public bool PasswordCorrect(CredentialDTO user)
  {
    var hashAndSalt = new KeyValuePair<byte[], byte[]>(
      this._provider.Connection.QueryFirstOrDefault<byte[]>("select password_hash from app_creds where username = @username", new { username = user.Username }),
      this._provider.Connection.QueryFirstOrDefault<byte[]>("select password_salt from app_creds where username = @username", new { username = user.Username })
    );

    using (HMACSHA512 hmac = new(hashAndSalt.Value)) {
      var hashToCheck = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
      return hashToCheck.SequenceEqual(hashAndSalt.Key);
    }
  }

  public bool UserExists(CredentialDTO user) {
    var result = this._provider.Connection.Query<Credential>(
      Queries.FetchUser,
      new { username = user.Username }
    );

    return result.Count() > 0;
  }
}