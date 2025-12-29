using System;
using System.ComponentModel.DataAnnotations;


namespace YuvaCep.Application.Dtos
{
    public class CreateAnnouncementDto
    {
        [StringLength(200, ErrorMessage = "Başlık en fazla 200 karakter olabilir")]
        public string Title { get; set; }

        [StringLength(2000, ErrorMessage = "İçerik en fazla 2000 karakter olabilir")]
        public string Content { get; set; }
        public Guid? ClassId { get; set; }

    }
}
