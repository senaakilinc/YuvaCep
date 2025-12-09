using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Domain.Entities
{
    public class Feedback
    {
        public Guid Id { get; set; }

        public Guid ParentId { get; set; }
        public Parent Parent { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        //type :"Suggestion", "Complaint", "Appreciation", "Question"
        [StringLength(50)]
        public string FeedBackType { get; set; } 

        [StringLength(200)]
        public string Subject { get; set; }

        [StringLength(2000)]
        public string Content { get; set; }

        // Öncelik: Low, Medium, High, Urgent
        [StringLength(20)]
        public string Priority { get; set; }

        //Öğretmenin yanıtı
        [StringLength(2000)]
        public string TeacherResponse { get; set; }

        public int? RespondedByTeacherId { get; set; }

        public Teacher Teacher { get; set; }

        public DateTime? RespondedAt { get; set; }
    }
}
