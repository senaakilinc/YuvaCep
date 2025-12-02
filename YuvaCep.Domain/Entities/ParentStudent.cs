using System;

namespace YuvaCep.Domain.Entities
{
    public class ParentStudent
    {
        // Köprünün bir ayağı: Veli
        public Guid ParentId { get; set; }
        public Parent Parent { get; set; } = null!;

        // Köprünün diğer ayağı: Çocuk
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        // Ekstra Bilgi: Atama Tarihi (Diyagramda vardı)
        public DateTime DateAssigned { get; set; }
    }
}