namespace AmoAsterisk.ApiManagement;

#nullable disable warnings
public class AddCallsPostBodyModel {
  public List<BareCallModel> add { get; set; } = new();
}

public abstract class BareCallModel {
  public string phone_number { get; set; }
  public string source { get; set; } = "test_asterisk";
  public long created_at { get; set; }
  public string created_by { get; set; }
  public int call_status { get; set; } = 4;
  public string call_result { get; set; } = "Разговор состоялся";
  public string direction { get; set; } = "outbound";
}

public class CallModelNoLinkNoDuration : BareCallModel {}

public class AddedCallModel : BareCallModel {
  public int duration { get; set; }
  public string? link { get; set; }
}