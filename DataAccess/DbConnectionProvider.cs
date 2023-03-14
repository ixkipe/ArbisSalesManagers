using MySql.Data.MySqlClient;

namespace ArbisSalesManagers.DataAccess;

public class DbConnectionProvider : IDbConnectionProvider
{
  public MySqlConnection Connection { get; set; }
  private readonly IConfiguration _configuration;

  public DbConnectionProvider(IConfiguration configuration)
  {
    this._configuration = configuration;
    this.Connection = new MySqlConnection(GetConnString());
  }

  private string GetConnString() {
    if (this._configuration.GetValue<bool>("AppConfig:LocalMachine")) {
      return this._configuration.GetConnectionString("Local");
    }

    return this._configuration.GetConnectionString("Remote");
  }

  public void Dispose()
  {
    this.Connection.Close();
  }
}