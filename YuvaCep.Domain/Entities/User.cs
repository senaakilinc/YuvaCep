using System;
using YuvaCep.Domain.Enums;

namespace YuvaCep.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty; 
        public string TCIDNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public string PasswordSalt { get; set; } = string.Empty;
        public UserRole Role { get; set; }

        public bool IsActive { get; set; }
    }
}