using System;

namespace YuvaCep.Domain.Entities
{
    public class TeacherClass
    {
        // Köprünün bir ayağı: Öğretmen
        public Guid TeacherId { get; set; }
        public Teacher Teacher { get; set; }

        // Köprünün diğer ayağı: Sınıf
        public Guid ClassId { get; set; }
        public Class Class { get; set; }
    }
}