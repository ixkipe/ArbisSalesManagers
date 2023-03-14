using MySql.Data.MySqlClient;

namespace ArbisSalesManagers.DataAccess;

public interface IDbConnectionProvider : IDisposable {
  MySqlConnection Connection { get; set; }
}