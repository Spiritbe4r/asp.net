using Microsoft.AspNetCore.Identity;

namespace api_rest.Models
{
    public class User : IdentityUser
    {
        public string CompanyName { get; set; }

        public string ProfileImageUrl { get; set; }
    }
}