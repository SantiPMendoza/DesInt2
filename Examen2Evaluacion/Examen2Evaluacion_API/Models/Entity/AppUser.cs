using Microsoft.AspNetCore.Identity;

namespace Examen2Evaluacion_API.Models.Entity
{
    public class AppUser : IdentityUser
    {

        public string Name { get; set; }


        public string Email { get; set; }

    }
}
