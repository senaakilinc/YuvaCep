using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Domain.Entities
{
    public class Parent
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

        // Öğrenci İlişkisi
        public ICollection<ParentStudent> ParentStudents { get; set; } = new List<ParentStudent>();
    }
}