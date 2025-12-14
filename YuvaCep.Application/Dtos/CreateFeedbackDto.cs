using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class CreateFeedbackDto
    {
        [Required(ErrorMessage = "Öğrenci seçilmelidir")]
        public Guid StudentId { get; set; }

        [Required(ErrorMessage = "Geri bildirim türü seçilmelidir")]
        [StringLength(50, ErrorMessage = "Geri bildirim türü en fazla 50 karakter olabilir")]
        public string FeedBackType { get; set; } // "Suggestion", "Complaint", "Appreciation", "Question"

        [Required(ErrorMessage = "Konu girilmelidir")]
        [StringLength(200, ErrorMessage = "Konu en fazla 200 karakter olabilir")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "İçerik girilmelidir")]
        [StringLength(2000, ErrorMessage = "İçerik en fazla 2000 karakter olabilir")]
        public string Content { get; set; }

        [StringLength(20, ErrorMessage = "Öncelik en fazla 20 karakter olabilir")]
        public string Priority { get; set; } = "Medium"; // "Low", "Medium", "High", "Urgent"
    }
}
