using System.Globalization;
using System.Text;
using AmoAsterisk.DbAccess.Models;
using Microsoft.Extensions.Configuration;

namespace AmoAsterisk;

public class RecordingLinkGenerator {
  private string baseUrl;

  public RecordingLinkGenerator(IConfiguration config)
  {
    this.baseUrl = config.GetSection("AppConfig").GetValue<string>("recordings_base_url");
  }

  public string GenerateRecordingLink(FinishedCallModel call) {
    if (string.IsNullOrEmpty(call.RecordingFile)) return string.Empty;

    StringBuilder sb = new();

    sb.Append(baseUrl);
    sb.Append(call.AddTime.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture));
    sb.Append("/");
    
    if (call.RecordingFile.Substring(call.RecordingFile.Length-4) == ".wav") {
      sb.Append(call.RecordingFile.Remove(call.RecordingFile.Length-4));
      sb.Append(".mp3");
    }
    else sb.Append(call.RecordingFile);
    
    return sb.ToString();
  }
}