using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Surname { get; set; } = string.Empty;

        [Required]
        public string ReferenceCode { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }
        public string? HealthNotes { get; set; }

        // SINIF
        public Guid ClassId { get; set; }
        public Class? Class { get; set; }

        // VELİLER (İşte burası! Listeyi burada başlatıyoruz, aşağıda Constructor YOK)
        public ICollection<ParentStudent> ParentStudents { get; set; } = new List<ParentStudent>();
    }
}