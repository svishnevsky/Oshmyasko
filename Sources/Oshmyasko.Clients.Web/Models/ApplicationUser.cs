using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Oshmyasko.Clients.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }
        
        public string Surname { get; set; }
    }
}
