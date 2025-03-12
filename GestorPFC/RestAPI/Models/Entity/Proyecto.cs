namespace RestAPI.Models.Entity
{
    public enum EstadoProyecto
    {
        Desarrollo,
        Aprobado,
        Suspendido,
        Revision
    }

    public class Proyecto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = null!;
        public string Descripcion { get; set; } = null!;

        public DateTime FechaEntrega { get; set; }
        public EstadoProyecto EstadoProyecto { get; set; }

        public int DepartamentoId { get; set; }
        public Departamento Departamento { get; set; } = null!;

        public virtual List<DateTime> FechasTutoria { get; set; } = new List<DateTime>();

        public int AlumnoId { get; set; }
        public Alumno Alumno { get; set; } = null!;


        public int? TutorProyectoId { get; set; }
        public Profesor TutorProyecto { get; set; } = null!;

      
        public Proyecto(Propuesta propuesta)
        {
            if (propuesta == null || propuesta.Estado != EstadoPropuesta.Aceptada || !propuesta.BooleanProyecto)
                throw new ArgumentException("La propuesta no es válida para crear un proyecto.");

            Titulo = propuesta.Titulo;
            Descripcion = propuesta.Descripcion;
            AlumnoId = propuesta.AlumnoId;

            Departamento = propuesta.Alumno.Curso.Departamento;
        }


        public Proyecto() { }
    }
}
