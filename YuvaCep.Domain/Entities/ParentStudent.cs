using System;
using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Domain.Entities
    
{
    public class ParentStudent
    {
        public Guid ParentId { get; set; }
        public Parent Parent { get; set; } = null!;

        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

    }
}