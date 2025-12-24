using System;
using YuvaCep.Domain.Enums; 

namespace YuvaCep.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } // Admin mi, Öğretmen mi?
        public string? TCIDNumber { get; set; }

        // Bu kullanıcı bir Öğretmen ise onun profilini tutar
        public Teacher? TeacherProfile { get; set; }

        // Bu kullanıcı bir Veli ise onun profilini tutar
        public Parent? ParentProfile { get; set; }
        public string PasswordSalt { get; set; } = string.Empty;
        public string ReferenceCode { get; set; } = string.Empty;
        public bool IsActive { get; set; }
    }
}