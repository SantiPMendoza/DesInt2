namespace AtecaAPI.Models.Entity
{
    public class Reserva
    {
        public int Id { get; set; }

        public DateOnly Fecha { get; set; }


        public int FranjaHorariaId { get; set; }
        public FranjaHoraria FranjaHoraria { get; set; } = null!;

        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; } = null!;

        public int GrupoClaseId { get; set; }
        public GrupoClase GrupoClase { get; set; } = null!;

        public string Estado { get; set; } = "Pendiente";

        public DateTime FechaSolicitud { get; set; } = DateTime.Now;
        public DateTime? FechaResolucion { get; set; }
    }


}
