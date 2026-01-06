namespace YuvaCep.Mobile.Dtos
{
    public class CompleteActivityResponseDto
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public NewBadgeDto NewBadge { get; set; } 
    }

    public class NewBadgeDto
    {
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
}