using System.ComponentModel.DataAnnotations;

namespace ExamenAPI.Models.DTOs
{
    public class TeacherDTO : CreateTeacherDTO
    {
        public int Id { get; set; }
    }

    public class CreateTeacherDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
