using System;
using System.Text.Json.Serialization;

namespace YuvaCep.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;
        public DateTime DateOfBirth { get; set; }
        public string ReferenceCode { get; set; } = string.Empty; // O meşhur 6 haneli kod
        public string HealthNotes { get; set; } = string.Empty;

        // Bu çocuk Hangi Sınıfta?
        public Guid ClassId { get; set; }
        public Class? Class { get; set; }
    }
}