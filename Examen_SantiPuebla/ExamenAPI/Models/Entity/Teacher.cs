namespace ExamenAPI.Models.Entity
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Relación N:N implícita con Course
        public List<Course> Courses { get; set; } = new();
    }
}
