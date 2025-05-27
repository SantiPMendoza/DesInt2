namespace AtecaAPI.Models.Entity
{
    public class Profesor
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!; // Solo correos @iescomercio.com
        public string GoogleId { get; set; } = null!;

        public ICollection<Reserva> Reservas { get; set; } = [];
    }

}
