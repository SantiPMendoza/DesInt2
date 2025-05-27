using Microsoft.AspNetCore.Identity;

namespace AtecaAPI.Models.Entity
{
    public class AppUser : IdentityUser
    {

        public string Name { get; set; } = null!;


    }
}
