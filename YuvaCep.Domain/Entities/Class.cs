using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // List için şart

namespace YuvaCep.Domain.Entities
{
    public class Class
    {
        public Guid Id { get; set; }
<<<<<<< Updated upstream

        public string Name { get; set; } = string.Empty; // Papatyalar
        public int YearLevel { get; set; } // 4 yaş
        public int MaxCapacity { get; set; } // Kontenjan 20

        // Bu sınıfta kimler var? (Sonra bağlayacağız)
        // public ICollection<Student> Students { get; set; }
=======
        [Required]
        public string Name { get; set; } = string.Empty;
        public int YearLevel { get; set; }
        public int MaxCapacity { get; set; }

        // Bu sınıfta kimler var? (Sonra bağlayacağız)
        public ICollection<Student> Students { get; set; } = new List<Student>();
        public ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();

>>>>>>> Stashed changes
    }
}
