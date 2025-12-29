using System;

namespace YuvaCep.Application.Dtos
{
    public class AnnouncementDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}