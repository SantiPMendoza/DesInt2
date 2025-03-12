namespace RestAPI.Models.Entity
{
    public class Departamento
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;

        public virtual ICollection<Profesor> Profesores { get; set; } = new List<Profesor>();
    }
}
