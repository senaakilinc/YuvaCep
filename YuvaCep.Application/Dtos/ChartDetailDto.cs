namespace YuvaCep.Application.Dtos
{
    public class ChartStudentDetailDto
    {
        public Guid StudentId { get; set; }
        public string StudentName { get; set; }
        public string PhotoUrl { get; set; }
        public int CompletedCount { get; set; }
        public bool IsCompletedToday { get; set; }
        public List<DateTime> CompletedDates { get; set; } 
    }

    public class ActivityChartDetailDto
    {
        public Guid ChartId { get; set; }
        public string Title { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public List<ChartStudentDetailDto> Students { get; set; }
    }

    public class StudentActivityChartDetailDto
    {
        public Guid Id { get; set; }         
        public Guid StudentId { get; set; }  
        public string Title { get; set; }    
        public int Month { get; set; }       
        public int Year { get; set; }
        public int Theme { get; set; }
        public List<DateTime> CompletedDates { get; set; } = new(); 
    }
}