namespace RestAPI.Models.Entity
{
    public enum EstadoPropuesta
    {
        StandBy,
        Aceptada,
        Denegada
    }

    public class Propuesta
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;

        public DateTime FechaEnvio { get; set; }
        public DateTime? FechaGestion { get; set; }

        public EstadoPropuesta Estado { get; set; }

        public string Departamento { get; set; } = null!;

        public bool BooleanProyecto { get; set; }

        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; } = null!;
    }
}
