namespace ArbisSalesManagers.AmoCrmApiRequests;

public interface IJwtHandler {
  string AccessToken { get; }
  void Refresh();
}