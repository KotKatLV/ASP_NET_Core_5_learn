using Microsoft.AspNetCore.Identity;

namespace Rocky.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    }
}