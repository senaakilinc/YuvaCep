using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Application.DTOs
{
    public class AssignBadgeRequest
    {
        [Required]
        public Guid StudentId { get; set; } // Hangi Öğrenciye veriyoruz?

        [Required]
        public Guid BadgeId { get; set; }   // O 3 rozetten hangisi? (Altın, Gümüş vb.)
    }
}