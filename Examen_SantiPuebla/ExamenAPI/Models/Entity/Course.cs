namespace ExamenAPI.Models.Entity
{
    public class Course
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;

        // Relación N:N implícita con Student
        public List<Student> Students { get; set; } = new();

        // Relación N:N implícita con Teacher
        public List<Teacher> Teachers { get; set; } = new();
    }
}
