using System.ComponentModel.DataAnnotations;

namespace YuvaCep.Application.Dtos
{
    public class LinkChildRequest
    {
        [Required(ErrorMessage = "Referans kodu zorunludur")]
        [StringLength(50)]
        public string ReferenceCode { get; set; }
    }
}