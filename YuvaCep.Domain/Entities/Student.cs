using System;
<<<<<<< Updated upstream
using System.Text.Json.Serialization;
=======
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
>>>>>>> Stashed changes

namespace YuvaCep.Domain.Entities
{
    public class Student
    {
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; } = string.Empty;
<<<<<<< Updated upstream
        public DateTime DateOfBirth { get; set; }
        public string ReferenceCode { get; set; } = string.Empty; // O meşhur 6 haneli kod
        public string HealthNotes { get; set; } = string.Empty;
=======
        [Required]
        public string Surname { get; set; } = string.Empty;
        [Required]
        public string ReferenceCode { get; set; } = string.Empty; // 6 haneli kod

        public string? HealthNotes { get; set; } 
        public DateTime DateOfBirth { get; set; }
>>>>>>> Stashed changes

        // Bu çocuk Hangi Sınıfta?
        public Guid ClassId { get; set; }
        public Class? Class { get; set; }
<<<<<<< Updated upstream
=======

        public ICollection<ParentStudent> ParentStudents { get; set; } = new List<ParentStudent>();

>>>>>>> Stashed changes
    }
}