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
        public Guid ParentId { get; set; } 
        public string? Surname { get; set; }
        public string? Gender { get; set; }

        [Required]
        public string TCIDNumber { get; set; } = string.Empty;

        [Required]
        public string ReferenceCode { get; set; } = string.Empty;

        public DateTime? DateOfBirth { get; set; }
        public string? HealthNotes { get; set; }
        public string? PhotoUrl { get; set; }

        // SINIF
        public Guid ClassId { get; set; }
        public Class? Class { get; set; }

        public ICollection<ParentStudent> ParentStudents { get; set; } = new List<ParentStudent>();
    }
}