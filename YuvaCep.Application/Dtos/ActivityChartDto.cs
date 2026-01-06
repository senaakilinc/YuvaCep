using YuvaCep.Domain.Enums;

namespace YuvaCep.Application.Dtos
{
    public class CreateActivityChartDto
    {
        public string Title { get; set; }
        public ChartTheme Theme { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }

    public class ActivityChartDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public ChartTheme Theme { get; set; }
        public bool IsCompletedToday { get; set; } 
        public int TotalCompletedCount { get; set; } 
    }

}