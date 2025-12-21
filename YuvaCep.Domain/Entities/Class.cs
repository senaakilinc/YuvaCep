using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Domain.Entities
{
    public class Class
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        public string YearLevel { get; set; }
        public int MaxCapacity { get; set; }

        // Öğrenciler
        public ICollection<Student> Students { get; set; } = new List<Student>();

        // Öğretmenler (İsim Düzeltildi: TeacherClass -> TeacherClasses)
        public ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();
    }
}