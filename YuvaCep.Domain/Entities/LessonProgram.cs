using System.Collections.Generic;

namespace YuvaCep.Domain.Entities
{

    public class LessonProgram
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<LessonProgramImage> Images { get; set; } = new List<LessonProgramImage>();
    }

    public class LessonProgramImage
    {
        public Guid Id { get; set; }
        public Guid LessonProgramId { get; set; }
        public string ImageBase64 { get; set; }
    }
}