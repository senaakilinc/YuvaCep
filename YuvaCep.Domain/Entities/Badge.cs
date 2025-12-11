using System;

namespace YuvaCep.Domain.Entities
{
    public class Badge
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;      // "Haftalık Rozet"
        public string Code { get; set; } = string.Empty;      // "WEEKLY", "GOLD" (Kodda kontrol için)
        public string ImagePath { get; set; } = string.Empty; // "/badges/gold.png"
    }
}