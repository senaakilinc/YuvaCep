using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class CreateAnnouncementDto
    {
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
        public string Title { get; set; }

        [StringLength(2000, ErrorMessage = "İçerik en fazla 2000 karakter olabilir")]
        public string Content { get; set; }

        public string TargetAudience { get; set; } // "All", "Class", "Individual"

        // TargetAudience = "Class" ise zorunlu
        public Guid? ClassId { get; set; }

        // TargetAudience = "Individual" ise zorunlu
        public Guid? StudentId { get; set; }
    }
}
