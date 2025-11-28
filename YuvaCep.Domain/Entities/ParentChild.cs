using System;

namespace YuvaCep.Domain.Entities
{
    public class ParentChild
    {
        // Köprünün bir ayağı: Veli
        public Guid ParentId { get; set; }
        public Parent Parent { get; set; }

        // Köprünün diğer ayağı: Çocuk
        public Guid ChildId { get; set; }
        public Child Child { get; set; }

        // Ekstra Bilgi: Atama Tarihi (Diyagramda vardı)
        public DateTime DateAssigned { get; set; }
    }
}