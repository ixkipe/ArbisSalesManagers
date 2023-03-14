using System.Diagnostics.CodeAnalysis;
using ArbisSalesManagers.Models;

namespace ArbisSalesManagers;

public class UserEqualityComparer : IEqualityComparer<User>
{
  public bool Equals(User? x, User? y) => x.Id == y.Id;

  public int GetHashCode([DisallowNull] User obj) => obj.Id.GetHashCode();
}