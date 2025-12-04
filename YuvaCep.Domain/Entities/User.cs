using System;
using YuvaCep.Domain.Enums; // Az önce yaptığın rol dosyasını çağırıyoruz

namespace YuvaCep.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; } // Kimlik No
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public UserRole Role { get; set; } // Admin mi, Öğretmen mi?
        public string? TCIDNumber { get; set; }

        // Bu kullanıcı bir Öğretmen ise onun profilini tutar
        public Teacher? TeacherProfile { get; set; }

        // Bu kullanıcı bir Veli ise onun profilini tutar
        public Parent? ParentProfile { get; set; }
    }
}