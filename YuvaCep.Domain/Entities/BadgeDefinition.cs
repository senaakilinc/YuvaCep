namespace YuvaCep.Domain.Entities
{
    public class BadgeDefinition
    {
        public Guid Id { get; set; }

        public string Name { get; set; }        
        public string Description { get; set; } 
        public string ImageUrl { get; set; }    
        public int TargetCount { get; set; }    

    }
}