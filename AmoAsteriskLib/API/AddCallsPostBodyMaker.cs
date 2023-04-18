using AmoAsterisk.DbAccess;
using AmoAsterisk;
using Newtonsoft.Json;
using AmoAsterisk.DbAccess.Models;
using Microsoft.Extensions.DependencyInjection;

namespace AmoAsterisk.ApiManagement;

public class AddCallsPostBodyMaker {
  private readonly IServiceProvider _services;

  public AddCallsPostBodyMaker(IServiceProvider services)
  {
    _services = services;
  }

  public async Task<string?> FormJsonRequestBody(DateTime? since = null) {
    var callFetcher = new CallFetcher(this._services);
    var calls = since is null? await callFetcher.GetCallsUpdateTime() : await callFetcher.GetCallsAfterCertainDate((DateTime)since);

    if (calls is null) return null;

    AddCallsPostBodyModel body = CreateBodyBase(calls);

    var request = JsonConvert.SerializeObject(body);
    return request;
  }

  private AddCallsPostBodyModel CreateBodyBase(IEnumerable<FinishedCallModel> calls) {
    AddCallsPostBodyModel body = new();
    var managers = this._services.GetService<AmoManagerList>().Managers;

    foreach (var call in calls) {
      switch (GetCallDisposition(call)) {
        case DispositionType.Answered:
          body.add.Add(new AddedCallModel() {
            phone_number = call.Dst.Remove(0, 2),
            created_at = (long)((call.AddTime.ToUniversalTime() - DateTime.UnixEpoch).TotalSeconds),
            created_by = FetchManagerId(call.Src, managers),
            duration = call.BillSec,
            link = new RecordingLinkGenerator(this._services.GetService<IConfiguration>()).GenerateRecordingLink(call)
          });
          break;
        case DispositionType.NoAnswer:
          body.add.Add(new CallModelNoLinkNoDuration() {
            phone_number = call.Dst.Remove(0, 2),
            created_at = (long)((call.AddTime.ToUniversalTime() - DateTime.UnixEpoch).TotalSeconds),
            created_by = FetchManagerId(call.Src, managers),
            call_status = 6,
            call_result = "Абонент не отвечает"
          });
          break;
        case DispositionType.Busy:
          body.add.Add(new CallModelNoLinkNoDuration() {
            phone_number = call.Dst.Remove(0, 2),
            created_at = (long)((call.AddTime.ToUniversalTime() - DateTime.UnixEpoch).TotalSeconds),
            created_by = FetchManagerId(call.Src, managers),
            call_status = 7,
            call_result = "Линия занята"
          });
          break;
      }
    }

    return body;
  }

  private string FetchManagerId(string phoneNumber, IEnumerable<AmoCrmUserModel> mgrs = null) {
    if (mgrs is null) {
      var managers = this._services.GetService<AmoManagerList>().Managers;
      return managers.Where(x => x.Num == phoneNumber).First().Id;
    }

    return mgrs.Where(x => x.Num == phoneNumber).First().Id;
  }

  private DispositionType GetCallDisposition(FinishedCallModel call) {
    if (call.Disposition.Contains("NO ANSWER")) return DispositionType.NoAnswer;
    else if (call.Disposition.Contains("ANSWERED")) return DispositionType.Answered;
    else if (call.Disposition.Contains("BUSY")) return DispositionType.Busy;
    else throw new ArgumentException("Unknown disposition type.");
  }
}