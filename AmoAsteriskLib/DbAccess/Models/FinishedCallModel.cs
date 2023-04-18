using System.ComponentModel.DataAnnotations;

namespace AmoAsterisk.DbAccess.Models;

#nullable disable warnings
public class FinishedCallModel {
  public DateTime Calldate { get; set; }
  public string Clid { get; set; }
  public string Src { get; set; }
  public string Dst { get; set; }
  public string Dcontext { get; set; }
  public string Channel { get; set; }
  public string DstChannel { get; set; }
  public string LastApp { get; set; }
  public string LastData { get; set; }
  public int Duration { get; set; }
  public int BillSec { get; set; }
  public string Disposition { get; set; }
  public int AmaFlags { get; set; }
  public string AccountCode { get; set; }
  public double UniqueId { get; set; }
  public string UserField { get; set; }
  public string Did { get; set; }
  public string RecordingFile { get; set; }
  public string Cnum { get; set; }
  public string Cnam { get; set; }
  public string Outbound_Cnum { get; set; }
  public string Outbound_Cnam { get; set; }
  public string Dst_Cnam { get; set; }
  public DateTime AddTime { get; set; }
  public string LinkedId { get; set; }
  public string PeerAccount { get; set; }
  public int Sequence { get; set; }
}