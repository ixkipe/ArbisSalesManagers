namespace AmoAsterisk.DbAccess;

public static class SldbMetaData {
  public const string FetchCreds = "select * from Creds";
  public const string UpdateCreds = "update Creds set access_token = @accessToken, refresh_token = @refreshToken where id = 2";
}