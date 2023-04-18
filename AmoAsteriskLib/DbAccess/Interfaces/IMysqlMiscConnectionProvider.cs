using MySql.Data.MySqlClient;

namespace AmoAsterisk.DbAccess;

public interface IMysqlMiscConnectionProvider {
  MySqlConnection Connection { get; }
}