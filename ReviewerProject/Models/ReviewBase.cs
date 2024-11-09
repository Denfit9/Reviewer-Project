namespace ReviewerProject.Models
{
    public abstract class ReviewBase
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required int Rating { get; set; }
        public bool WasUpdated { get; set; } = false;
        public DateTime CreatedDate { get; set; }
    }
}
