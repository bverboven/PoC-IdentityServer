using Microsoft.AspNetCore.Identity;

namespace Identity.Library.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
    }
}
