using ExamenWPF.Models.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamenWPF.Models
{
    public class CourseDTO : CreateCourseDTO
    {
        public int Id { get; set; }

        public List<StudentDTO> Students { get; set; } = new();
        public List<TeacherDTO> Teachers { get; set; } = new();

        public string StudentIdsDisplay => string.Join(", ", Students.Select(s => s.Id));
        public string TeacherIdsDisplay => string.Join(", ", Teachers.Select(t => t.Id));


    }

    public class CreateCourseDTO
    {
        [Required]
        public string Title { get; set; }
        public List<int> StudentIds { get; set; } = new();
        public List<int> TeacherIds { get; set; } = new();
    }
}
