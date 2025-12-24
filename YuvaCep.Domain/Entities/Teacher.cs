using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YuvaCep.Domain.Enums;

namespace YuvaCep.Domain.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string TCIDNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
        public string PasswordSalt { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public DateTime HireDate { get; set; }

        // User İlişkisi
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Sınıflar (Modern Başlatma)
        public ICollection<TeacherClass> TeacherClasses { get; set; } = new List<TeacherClass>();
    }
}