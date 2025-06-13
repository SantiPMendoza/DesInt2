using System.ComponentModel.DataAnnotations;

namespace ExamenAPI.Models.DTOs
{
    public class CourseDTO : CreateCourseDTO
    {
        public int Id { get; set; }

        public List<StudentDTO> Students { get; set; } = new();
        public List<TeacherDTO> Teachers { get; set; } = new();

    }

    public class CreateCourseDTO
    {
        [Required]
        public string Title { get; set; }
        public List<int> StudentIds { get; set; } = new();
        public List<int> TeacherIds { get; set; } = new();
    }
}
