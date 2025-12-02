using System;
using System.Text.Json.Serialization;

namespace YuvaCep.Domain.Entities
{
    public class LessonProgram
    {
        public Guid Id { get; set; }

        // Örn: "Nisan İlk Hafta Ders Programı"
        public string Title { get; set; } = string.Empty;

        public string ImagePath { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid ClassId { get; set; }

        [JsonIgnore]
        public Class? Class { get; set; }
    }
}