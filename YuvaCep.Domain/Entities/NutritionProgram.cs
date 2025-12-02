using System;
using System.Text.Json.Serialization;

namespace YuvaCep.Domain.Entities
{
    public class NutritionProgram
    {
        public Guid Id { get; set; }

        // Örn: "Mart Ayı Yemek Listesi" (Öğretmen elle yazacak)
        public string Title { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty; // Resim

        // Sadece bilgi amaçlı "Ne zaman yüklendi?" (Otomatik atacağız)
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Hangi Sınıfın Listesi?
        public Guid ClassId { get; set; }

        [JsonIgnore]
        public Class? Class { get; set; }
    }
}