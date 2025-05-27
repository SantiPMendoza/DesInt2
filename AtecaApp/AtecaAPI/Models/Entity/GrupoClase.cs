namespace AtecaAPI.Models.Entity
{
    public class GrupoClase
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public ICollection<Reserva> Reservas { get; set; } = [];
    }

}
