namespace ExamenApi.Models
{
    public class Juego
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // Foreign key to AppUser
        public string AppUserId { get; set; }
        public AppUser AppUser { get; set; }

        public double Resultado { get; set; }
        public DateTime Fecha_Inicio { get; set; }
        public DateTime Fecha_Fin { get; set; }
    }
}
