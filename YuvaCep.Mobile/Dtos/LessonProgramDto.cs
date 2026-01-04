namespace YuvaCep.Mobile.Dtos
{
    public class LessonProgramDto
    {
        public Guid Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<LessonProgramImageDto> Images { get; set; } = new();
    }

    public class LessonProgramImageDto
    {
        public Guid Id { get; set; }
        public string ImageBase64 { get; set; }
    }
}