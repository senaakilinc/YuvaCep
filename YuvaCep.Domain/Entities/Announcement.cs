using System;

namespace YuvaCep.Domain.Entities
{
    public class Announcement
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public Guid? TargetClassId { get; set; }
        public Class? TargetClass { get; set; }
    }
}