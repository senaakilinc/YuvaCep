using YuvaCep.Domain.Enums;

namespace YuvaCep.Domain.Entities
{
    public class ActivityChart
    {
        public Guid Id { get; set; }
        public Guid ClassId { get; set; } 
        public string Title { get; set; } 
        public ChartTheme Theme { get; set; } 
        public int Month { get; set; }
        public int Year { get; set; }
        public DateTime CreatedAt { get; set; }

        public ICollection<StudentChartEntry> Entries { get; set; } = new List<StudentChartEntry>();
    }
}