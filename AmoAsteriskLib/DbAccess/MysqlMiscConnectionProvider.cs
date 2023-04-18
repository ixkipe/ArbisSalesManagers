using AmoAsterisk.DbAccess;
using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace AmoAsterisk.DbAccess;

public class MysqlMiscConnectionProvider : IMysqlMiscConnectionProvider
{
  public MySqlConnection Connection { get; }

  public MysqlMiscConnectionProvider(IConfiguration config)
  {
    if (config.GetSection("AppConfig").GetValue<bool>("LocalMachine")) {
      this.Connection = new MySqlConnection(config.GetSection("ConnectionStrings").GetValue<string>("Local"));
    }
    else {
      this.Connection = new MySqlConnection(config.GetSection("ConnectionStrings").GetValue<string>("Remote"));
    }
  }
}