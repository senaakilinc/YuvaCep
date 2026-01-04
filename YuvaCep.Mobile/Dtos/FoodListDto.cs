namespace YuvaCep.Mobile.Dtos
{
    public class FoodListDto
    {
        public Guid Id { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public string ImageBase64 { get; set; }
    }
}