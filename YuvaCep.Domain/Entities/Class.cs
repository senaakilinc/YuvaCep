using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Domain.Entities
{
    public class Class
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
        public string AgeGroup { get; set; }
        public Guid TeacherId { get; set; } 
        public Teacher Teacher { get; set; }

        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();
    }
}