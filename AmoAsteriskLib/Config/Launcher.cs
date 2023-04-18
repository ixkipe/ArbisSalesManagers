using AmoAsterisk.ApiManagement;
using AmoAsterisk.DbAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace AmoAsterisk;

public class Launcher {
  private readonly IServiceProvider _serviceProvider;
  private readonly AmoCrmApiManager _manager;
  public Launcher(IServiceProvider serviceProvider)
  {
    this._serviceProvider = serviceProvider;
    this._manager = new(
      serviceProvider.GetService<IConfiguration>(),
      serviceProvider
    );
  }
  public async Task Launch() {
    try {
      while (true) {
        await this._manager.AddCallsContinuously(false);
      }
    }
    catch (Exception e) {
      Log.Fatal(exception: e, "Something went wrong.");
      await Task.Delay(1000);
      Environment.Exit(1);
    }
  }

  public async Task AddCalls() {
    await this._manager.AddCallsContinuously(false);
  }
}