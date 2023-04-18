using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AmoAsterisk.DbAccess;

public static class Queries {
  public static string JoinNumbers(this IEnumerable<AmoCrmUserModel> managers) {
    List<string> phones = new List<string>();
    foreach (var manager in managers) {
      phones.Add(manager.Num);
    }

    return string.Join(", ", phones);
  }
}