using System.Reflection;
using AmoAsterisk.ApiManagement;
using AmoAsterisk.DbAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Core;

namespace AmoAsterisk;

public static class Configurator {
  public async static Task<IServiceProvider> Configure(this Assembly assembly, bool isDebug = false, IServiceProvider serviceProvider = null, Logger serilogLogger = null, IMysqlMiscConnectionProvider mysqlMiscConnectionProvider = null) {
    if (serviceProvider is not null) {
      // in this case the app is monolithic
      Log.Logger ??= serilogLogger ?? new LoggerConfiguration().ReadFrom.Configuration(serviceProvider.GetService<IConfiguration>()).CreateLogger();
      return serviceProvider;
    }
    
    var configBuilder = new ConfigurationBuilder().SetBasePath(isDebug ? Path.GetDirectoryName(assembly.Location) : AppDomain.CurrentDomain.BaseDirectory)
      .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      .AddEnvironmentVariables()
      .Build();
    
    // also add Seq
    Log.Logger = serilogLogger ?? new LoggerConfiguration().ReadFrom.Configuration(configBuilder).CreateLogger();

    var host = Host.CreateDefaultBuilder().ConfigureHostConfiguration(config => config.AddConfiguration(configBuilder)).ConfigureServices((context, services) => {
      // dependency injection and other shit goes here
      services.AddSingleton<IDbConnectionProvider>(implementationInstance: new DefaultDbConnectionProvider(configBuilder));
      services.AddSingleton<IMysqlMiscConnectionProvider>(implementationInstance: mysqlMiscConnectionProvider ?? new MysqlMiscConnectionProvider(configBuilder));
      services.AddSingleton<AmoManagerList>(implementationInstance: new AmoManagerList(configBuilder));
    }).UseSerilog().Build();

    return serviceProvider;
  }
}