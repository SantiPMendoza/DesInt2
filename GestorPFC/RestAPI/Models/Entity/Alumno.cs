namespace RestAPI.Models.Entity
{
    public class Alumno
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellidos { get; set; } = null!;
        public string Email { get; set; } = null!;

        public int CursoId { get; set; }
        public Curso Curso { get; set; } = null!;
    }
}
