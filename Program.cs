Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", Serilog.Events.LogEventLevel.Information)
                .MinimumLevel.Override("Microsoft.AspNetCore", Serilog.Events.LogEventLevel.Warning)
                .WriteTo.File("applog.log")
                .WriteTo.Console()
                .CreateLogger();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddSingleton<IDbConnectionProvider, DbConnectionProvider>();
builder.Services.AddScoped<ArbisSalesManagers.AmoCrmApiRequests.IJwtHandler, ArbisSalesManagers.AmoCrmApiRequests.JwtHandler>();
builder.Services.AddScoped<IUserValidator, UserValidator>();
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
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

app.MapDefaultControllerRoute();

app.UseSerilogRequestLogging(opt => {
  opt.MessageTemplate = "{RemoteIpAddress} {RequestScheme} {RequestHost} {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
  opt.EnrichDiagnosticContext = (diagnosticContext, httpContext) => {
    diagnosticContext.Set("RemoteIpAddress", httpContext.Connection.RemoteIpAddress);
    diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
    diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
  };
});

app.Run();