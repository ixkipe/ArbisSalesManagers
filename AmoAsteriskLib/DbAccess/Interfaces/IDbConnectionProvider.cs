using MySql.Data.MySqlClient;

namespace AmoAsterisk.DbAccess;

public interface IDbConnectionProvider {
  MySqlConnection Connection { get; }
}