using Microsoft.AspNetCore.Identity;

namespace ExamenApi.Models
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }

        // Relationship
        public ICollection<Juego> Juegos { get; set; } = new List<Juego>();
    }
}
