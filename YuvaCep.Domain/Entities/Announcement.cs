using System;

namespace YuvaCep.Domain.Entities
{
    public class Announcement
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        // Class için
        public Guid? ClassId { get; set; }
        public Class Class { get; set; }
    }
}