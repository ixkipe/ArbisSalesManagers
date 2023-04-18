using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace ArbisSalesManagers;

public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
{
  public bool Authorize([NotNull] DashboardContext context) => 
    context.GetHttpContext().User.HasClaim(x => x.Type == ClaimTypes.Name && x.Value == "admin");
}