namespace AmoAsterisk.DbAccess;

public class QueryManager {
  public const string GetLastCallTime = "select time_and_date from MostRecentCallTime limit 1";
  public const string UpdateLastCallTime = "update MostRecentCallTime set time_and_date = @dateTime where id = 1";
  private readonly IMysqlMiscConnectionProvider _mysql;

  public QueryManager(IMysqlMiscConnectionProvider mysql)
  {
    _mysql = mysql;
  }

  public string GetCallsAfterCertainDate() {
    var managers = this._mysql.Connection.Query<AmoCrmUserModel>(
      ArbisSalesManagers.DataAccess.Queries.GetAllManagers
    );

    return $"select * from {MetaData.DatabaseName} where (disposition = {MetaData.CallAnswered} and billsec >= {MetaData.DurationSecondsMin}) and src in ({managers.JoinNumbers()}) and length(dst) > 5 and addtime > @from order by addtime desc";
  }

  public string GetUnansweredCallsAfterCertainDate() {
    var managers = this._mysql.Connection.Query<AmoCrmUserModel>(
      ArbisSalesManagers.DataAccess.Queries.GetAllManagers
    );

    return $"select * from {MetaData.DatabaseName} where (disposition = {MetaData.CallsLineBusy} or disposition = {MetaData.CallsNoAnswer}) and src in ({managers.JoinNumbers()}) and length(dst) > 5 and addtime > @from order by addtime desc";
  }

  
}