namespace YuvaCep.Domain.Entities
{
    public class StudentChartEntry
    {
        public Guid Id { get; set; }

        public Guid ActivityChartId { get; set; } 
        public ActivityChart ActivityChart { get; set; }

        public Guid StudentId { get; set; } 

        public DateTime Date { get; set; } 
        public bool IsCompleted { get; set; } 
    }
}