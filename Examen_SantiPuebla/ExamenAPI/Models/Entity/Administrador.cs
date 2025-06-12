using Microsoft.AspNetCore.Identity;

namespace ExamenAPI.Models.Entity
{
    public class Administrador
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public string AppUserId { get; set; } = null!; // FK a Identity
        public AppUser AppUser { get; set; } = null!;

         // Email del administrador, no es obligatorio que sea el mismo que el de Identity
    }


}
