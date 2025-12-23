using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class RespondFeedbackDto
    {
        [Required(ErrorMessage = "Yanıt girilmelidir")]
        [StringLength(2000, ErrorMessage = "Yanıt en fazla 2000 karakter olabilir")]
        public string TeacherResponse { get; set; }
    }
}
