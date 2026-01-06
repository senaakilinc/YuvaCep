namespace YuvaCep.Domain.Entities
{
    public class StudentBadge
    {
        public Guid Id { get; set; }

        public Guid StudentId { get; set; }

        public Guid BadgeDefinitionId { get; set; }
        public BadgeDefinition BadgeDefinition { get; set; }

        public DateTime EarnedDate { get; set; }
    }
}