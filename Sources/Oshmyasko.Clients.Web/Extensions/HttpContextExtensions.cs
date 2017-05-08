using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Oshmyasko.Clients.Web.Models;

namespace Oshmyasko.Clients.Web.Extensions
{
    public static class HttpContextExtensions
    {
        public static ApplicationUser GetUser(this HttpContext context)
        {
            return context.User.Identity.IsAuthenticated 
                ? ((UserManager<ApplicationUser>)context.RequestServices.GetService(typeof(UserManager<ApplicationUser>))).FindByNameAsync(context.User.Identity.Name).Result
                : null;
        }
    }
}
