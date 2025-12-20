using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class AnnouncementResponseDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string TargetAudience { get; set; }
        public string TargetAudienceDisplay { get; set; } // "Tüm Veliler", "Papatyalar Sınıfı", "Ahmet Yılmaz"

        public Guid? ClassId { get; set; }
        public string ClassName { get; set; }

        public Guid? StudentId { get; set; }
        public string StudentName { get; set; }

        public Guid TeacherId { get; set; }
        public string TeacherName { get; set; }

        public DateTime CreatedAt { get; set; }

        public bool NotificationSent { get; set; }
        public int? RecipientCount { get; set; }
    }
}
