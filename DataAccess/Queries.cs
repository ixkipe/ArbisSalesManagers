namespace ArbisSalesManagers.DataAccess;

public class Queries {
  public const string GetAllManagers = $"select * from {MetaData.ManagersTable}";
  public const string GetManagerUnderId = $"select * from {MetaData.ManagersTable} where id = @id";
  public const string GetInactiveManagerUnderId = $"select * from {MetaData.ManagersTable} where id = @id";
  public const string CreateManager = $"insert into {MetaData.ManagersTable} values (@id, @username, @num)";
  public const string CreateManagerInInactive = $"insert into {MetaData.InactiveManagersTable} values (@id, @username, @num)";
  public const string RemoveManager = $"delete from {MetaData.ManagersTable} where id = @id";
  public const string GetAllInactiveManagers = $"select * from {MetaData.InactiveManagersTable}";
  public const string MoveManagerToInactive = $"insert into {MetaData.InactiveManagersTable} select * from {MetaData.ManagersTable} where id = @id";
  public const string MoveManagerToActive = $"insert into {MetaData.ManagersTable} select * from {MetaData.InactiveManagersTable} where id = @id";
  public const string RemoveManagerFromInactive = $"delete from {MetaData.InactiveManagersTable} where id = @id";
  public const string UpdateManagerNumber = $"update {MetaData.ManagersTable} set num = @num where id = @id";

  // what follows applies to authentication
  public const string FetchCreds = $"select * from {MetaData.ApplicationCreds} where username = 'admin'";
  public const string FetchAmoAccessToken = $"select access_token from {MetaData.AmocrmCreds} where id = 2";
  public const string FetchAmoRefreshToken = $"select refresh_token from {MetaData.AmocrmCreds} where id = 2";
  public const string UpdateAmoWebCreds = $"update {MetaData.AmocrmCreds} set access_token = @acc, refresh_token = @refr where id = 2";
  
  public const string FetchUser = $"select * from {MetaData.ApplicationCreds} where username = @username";
  public const string CheckIfUserExists = $"select count(*) from {MetaData.ApplicationCreds} where username = @username";
  public const string Register = $"insert into {MetaData.ApplicationCreds} values (@username, @salt, @hash, @is_admin)";
}