namespace YuvaCep.Mobile.Dtos
{
    public class ActivityChartDetailDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string Title { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }

        public List<DateTime> CompletedDates { get; set; } = new();

        public List<ChartStudentDetailDto> Students { get; set; } = new();
    }

    public class ChartStudentDetailDto
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string PhotoUrl { get; set; }
        public int CompletedCount { get; set; }
        public bool IsCompletedToday { get; set; }
        public List<DateTime> CompletedDates { get; set; }
    }
}