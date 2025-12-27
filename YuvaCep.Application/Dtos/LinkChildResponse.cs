using System;

namespace YuvaCep.Application.Dtos
{
    public class LinkChildResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
    }
}