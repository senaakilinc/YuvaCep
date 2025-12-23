namespace YuvaCep.Application.DTOs
{
    public class StudentBadgeResponse
    {
        public string BadgeName { get; set; } = string.Empty; // Örn: "Haftanın Yıldızı"
        public string ImagePath { get; set; } = string.Empty; // Örn: "star_gold.png"
        public DateTime EarnedAt { get; set; }                // Ne zaman kazandı?
    }
}