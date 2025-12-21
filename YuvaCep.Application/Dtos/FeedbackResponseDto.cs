using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class FeedbackResponseDto
    {
        public Guid Id { get; set; }
        public Guid ParentId { get; set; }
        public string ParentName { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public string FeedBackType { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
        public string Priority { get; set; }

        // Öğretmen Yanıtı
        public string TeacherResponse { get; set; }
        public Guid? RespondedByTeacherId { get; set; }
        public string TeacherName { get; set; }
        public DateTime? RespondedAt { get; set; }

        // Durum
        public bool IsAnswered => !string.IsNullOrEmpty(TeacherResponse);
    }
}
