namespace RestAPI.Models.Entity
{
    public class Curso
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Alumno> Alumnos { get; set; } = new List<Alumno>();

        public virtual ICollection<Profesor> Profesores { get; set; } = new List<Profesor>();

        public int DepartamentoId { get; set; }
        public Departamento Departamento { get; set; } = null!;


        public int TutorId { get; set; }
        public Profesor Tutor { get; set; } = null!;
    }
}
