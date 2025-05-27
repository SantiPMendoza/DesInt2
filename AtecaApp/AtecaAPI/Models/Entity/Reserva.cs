namespace AtecaAPI.Models.Entity
{
    public class Reserva
    {
        public int Id { get; set; }

        public DateOnly Fecha { get; set; }
        public TimeOnly HoraInicio { get; set; }
        public TimeOnly HoraFin { get; set; }

        public int ProfesorId { get; set; }
        public Profesor Profesor { get; set; } = null!;

        public int GrupoClaseId { get; set; }
        public GrupoClase GrupoClase { get; set; } = null!;

        public string Estado { get; set; } = "Pendiente"; // "Pendiente", "Aprobada", "Rechazada", "Cancelada"

        public DateTime FechaSolicitud { get; set; } = DateTime.UtcNow;
        public DateTime? FechaResolucion { get; set; } // Cuando se aprueba/rechaza
    }

}
