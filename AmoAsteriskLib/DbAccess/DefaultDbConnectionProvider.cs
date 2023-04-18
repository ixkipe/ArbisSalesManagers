using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;

namespace AmoAsterisk.DbAccess;

public class DefaultDbConnectionProvider : IDbConnectionProvider
{
  public MySqlConnection Connection { get; }

  public DefaultDbConnectionProvider(IConfiguration config)
  {
    this.Connection = new(config.GetSection("AppConfig").GetValue<string>("mysql_connection_string"));
  }
}