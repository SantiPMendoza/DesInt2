namespace AtecaAPI.Models.Entity
{
    public class Administrador
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public string AppUserId { get; set; } = null!; // FK a Identity
        public AppUser AppUser { get; set; } = null!;
    }


}
