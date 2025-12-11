using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Domain.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; }

<<<<<<< Updated upstream
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string TC_IdNumber { get; set; } = string.Empty;
=======
        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        public string TCIDNumber { get; set; } = string.Empty; 
>>>>>>> Stashed changes
        public string PhoneNumber { get; set; } = string.Empty;

        // Hangi User'a bağlı?
        public Guid UserId { get; set; }
<<<<<<< Updated upstream
        public User? User { get; set; }
=======
        public User User { get; set; } = null!;

        public ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();
>>>>>>> Stashed changes
    }
}