using System;

namespace YuvaCep.Domain.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string TC_IdNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // Hangi User'a bağlı?
        public Guid UserId { get; set; }
        public User? User { get; set; }
    }
}