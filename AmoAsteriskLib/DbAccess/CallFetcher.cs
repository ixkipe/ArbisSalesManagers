using System.Globalization;
using AmoAsterisk.DbAccess.Models;
using Dapper;
using Microsoft.Extensions.DependencyInjection;
using MySql.Data.MySqlClient;
using Serilog;

namespace AmoAsterisk.DbAccess;

public class CallFetcher {
  private IServiceProvider _services;

  public CallFetcher(IServiceProvider services)
  {
    _services = services;
  }

  /// <summary>
  /// Fetch calls that have been added to the database after a specific time.
  /// </summary>
  /// <param name="from">Date and time from which to get calls.</param>
  /// <returns>An iterator of calls if any are found; otherwise, NULL.</returns>
  public async Task<IEnumerable<FinishedCallModel>?> GetCallsAfterCertainDate(DateTime from) {
    var calls = await this._services.GetService<IDbConnectionProvider>().Connection.QueryAsync<FinishedCallModel>(
      new QueryManager(this._services.GetService<IMysqlMiscConnectionProvider>()).GetCallsAfterCertainDate(),
      new {from = from}
    );

    // 13.12.2022 added this feature upon request to add unresponded calls
    var unansweredCalls = await this._services.GetService<IDbConnectionProvider>().Connection.QueryAsync<FinishedCallModel>(
      new QueryManager(this._services.GetService<IMysqlMiscConnectionProvider>()).GetUnansweredCallsAfterCertainDate(),
      new {from = from}
    );

    if (calls.Count() > 0) {
      foreach (var call in calls) {
        Log.Information($"{call.AddTime.ToString(format: "dd MMMM yyyy HH:mm:ss")} {call.Src} звонил(а) {call.Dst}. Длительность: {new TimeSpan(0, 0, call.BillSec).ToString("mm':'ss")}.");
      }
    }

    if (unansweredCalls.Count() > 0) {
      foreach (var call in unansweredCalls) {
        Log.Information($"{call.AddTime.ToString(format: "dd MMMM yyyy HH:mm:ss")} {call.Src} звонил(а) {call.Dst}. Нет ответа.");
      }
    }

    calls = calls.Concat(unansweredCalls).OrderByDescending(x => x.AddTime);

    return calls.Any()? calls : null;
  }

  public async Task<IEnumerable<FinishedCallModel>?> GetCallsUpdateTime() {
      var time = await this._services.GetService<IMysqlMiscConnectionProvider>().Connection.QueryFirstAsync<DateTime>(QueryManager.GetLastCallTime);

      Log.Information(String.Format("{0}", time));
      var calls = await GetCallsAfterCertainDate(time);

      if (calls is not null) {
        // time = calls.First().AddTime;
        // await this._services.GetService<IMysqlMiscConnectionProvider>().Connection.ExecuteAsync(
        //   QueryManager.UpdateLastCallTime,
        //   new {
        //     dateTime = time
        //   }
        // );
        return calls;
      }

      return null;
  }

  public async static Task UpdateReferenceTime(IEnumerable<FinishedCallModel>? calls, IServiceProvider services) {
    if (calls is null) return;

    var time = calls.First().AddTime;
    await services.GetService<IMysqlMiscConnectionProvider>().Connection.ExecuteAsync(
      QueryManager.UpdateLastCallTime,
      new {
        dateTime = time
      }
    );
  }
}