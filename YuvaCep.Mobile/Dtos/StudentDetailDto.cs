using System;
using System.Collections.Generic;
using System.Text;

namespace YuvaCep.Mobile.Dtos
{
    public class StudentDetailDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string PhotoUrl { get; set; }
        public string ParentName { get; set; }
        public string ClassName { get; set; }
        public string Gender { get; set; }
        public string TCIDNumber { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string HealthNotes { get; set; }
    }
}
