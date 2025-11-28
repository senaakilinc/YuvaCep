using System;

namespace YuvaCep.Domain.Entities
{
    public class Teacher
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
        public string Surname { get; set; }
        public string TC_IdNumber { get; set; }
        public string PhoneNumber { get; set; }

        // Hangi User'a bağlı?
        public Guid UserId { get; set; }
        public User User { get; set; }
    }
}