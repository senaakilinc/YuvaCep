namespace YuvaCep.Application.Dtos
{
    public class FoodListDto
    {
        public Guid Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string ImageBase64 { get; set; }
    }

    public class CreateFoodListDto
    {
        public DateTime Date { get; set; } 
        public string ImageBase64 { get; set; }
    }
}