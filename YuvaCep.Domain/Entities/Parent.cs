using System.ComponentModel.DataAnnotations;
using YuvaCep.Domain.Enums;

namespace YuvaCep.Domain.Entities
{
    public class Parent
    {
        public Guid Id { get; set; }

        [Required]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        public string LastName { get; set; } = string.Empty;

        public string TCIDNumber { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;// HashingHelper için
        public string PasswordSalt { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool IsActive { get; set; }
        public string ReferenceCode { get; set; } = string.Empty; // Öğrenci eşleşmesi için

        // User İlişkisi
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Öğrenci İlişkisi
        public ICollection<ParentStudent> ParentStudents { get; set; } = new List<ParentStudent>();
    }
}