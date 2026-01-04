namespace YuvaCep.Domain.Entities
{
    public class FoodList
    {
        public Guid Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string ImageBase64 { get; set; } 
        public Guid ClassId { get; set; } 
        public DateTime CreatedAt { get; set; }
    }
}