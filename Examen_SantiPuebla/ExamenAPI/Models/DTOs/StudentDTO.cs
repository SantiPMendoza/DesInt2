using System.ComponentModel.DataAnnotations;

namespace ExamenAPI.Models.DTOs
{
    public class StudentDTO : CreateStudentDTO
    {
        public int Id { get; set; }
    }

    public class CreateStudentDTO
    {
        [Required]
        public string Name { get; set; }
    }
}
