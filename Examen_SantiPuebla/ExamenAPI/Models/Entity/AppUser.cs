using Microsoft.AspNetCore.Identity;

namespace ExamenAPI.Models.Entity
{
    public class AppUser : IdentityUser
    {

        public string Name { get; set; } = null!;


    }
}
