using AmoAsterisk;
using AmoAsterisk.ApiManagement;
using AmoAsterisk.DbAccess;
using ArbisSalesManagers;
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.DataProtection;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
  .ReadFrom.Configuration(builder.Configuration)
  .CreateLogger();

builder.Services.AddControllersWithViews();
builder.Services.AddHangfire(c => {
  c.UseRecommendedSerializerSettings()
  .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
  .UseSimpleAssemblyNameTypeSerializer()
  .UseSQLiteStorage(builder.Configuration.GetConnectionString("HangfireSqlite"));
});
builder.Services.AddHangfireServer();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IMysqlMiscConnectionProvider>(implementationInstance: new MysqlMiscConnectionProvider(builder.Configuration));
builder.Services.AddSingleton<IDbConnectionProvider>(implementationInstance: new DefaultDbConnectionProvider(builder.Configuration));
builder.Services.AddScoped<IUserValidator, UserValidator>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<AmoManagerList>(implementationInstance: new AmoManagerList(builder.Configuration));
// builder.Services.AddScoped<AmoCrmApiManager>(implementationFactory: (svc) => new AmoCrmApiManager(builder.Configuration, svc));
builder.Services.AddDataProtection().PersistKeysToFileSystem(directory: new DirectoryInfo("keys"));
builder.Services.AddAuthentication(defaultScheme: "AmoAsteriskAuthCookie").AddCookie("AmoAsteriskAuthCookie", opt => {
  opt.Cookie.Name = "AmoAsteriskAuthCookie";
  opt.ExpireTimeSpan = TimeSpan.FromDays(30d);
  opt.LoginPath = "/Login";
  opt.AccessDeniedPath = "/AccessDenied";
});
builder.Services.AddAuthorization(opt => {
  opt.AddPolicy("MustBeAdmin", policy => policy.RequireClaim(ClaimTypes.Name, "admin"));
  opt.AddPolicy("RegularUser", policy => policy.RequireClaim(ClaimTypes.Name, "admin", "user"));
});
builder.Host.UseSerilog();

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions() {
  ForwardedHeaders = Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedFor | Microsoft.AspNetCore.HttpOverrides.ForwardedHeaders.XForwardedProto
});

if (app.Environment.IsDevelopment()) {
  app.UseHttpsRedirection();
  app.UseDeveloperExceptionPage();
}

if (app.Environment.IsProduction()) {
  app.UseExceptionHandler("/Home/Error");
}

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapHangfireDashboard();
app.UseHangfireDashboard(options: new DashboardOptions(){
  Authorization = new[] { new HangfireAuthorizationFilter() }
});
app.MapDefaultControllerRoute();

app.UseSerilogRequestLogging(opt => {
  opt.MessageTemplate = "{RemoteIpAddress} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
  opt.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
    diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
  };
});

if (app.Environment.IsProduction()) {
}
  RecurringJob.AddOrUpdate<AmoCrmApiManager>(
    recurringJobId: "add-calls",
    methodCall: apiManager => apiManager.AddCallsContinuously(false),
    cronExpression: Cron.Minutely
  );

app.Run();