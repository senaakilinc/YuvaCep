using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Domain.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        public string TCIDNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // User İlişkisi
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Sınıflar (Modern Başlatma)
        public ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();
    }
}