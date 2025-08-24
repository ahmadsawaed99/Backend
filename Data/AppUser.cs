using Microsoft.AspNetCore.Identity;

namespace Backend.Data
{
    public class AppUser : IdentityUser
    {
        public string? Name { get; set; }
    }
}
