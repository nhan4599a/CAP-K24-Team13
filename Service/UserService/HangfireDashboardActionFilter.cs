using Hangfire.Annotations;
using Hangfire.Dashboard;

namespace UserService
{
    public class HangfireDashboardActionFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize([NotNull] DashboardContext context)
        {
            return true;
        }
    }
}
