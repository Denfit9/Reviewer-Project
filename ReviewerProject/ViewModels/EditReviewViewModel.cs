using ReviewerProject.Models;

namespace ReviewerProject.ViewModels
{
    public class EditReviewViewModel
    {
        public Guid Id { get; set; }
        [MaxLength(80, ErrorMessage = "Maximum 80 character long")]
        public string? Name { get; set; }
        [MaxLength(1099, ErrorMessage = "Maximum 1099 character long")]
        public string? Description { get; set; }
        public int? Rating { get; set; }
        public List<int> Ratings { get; set; }
        public Guid? ReviewingTypeKey { get; set; }
        public List<ReviewingType> ReviewingTypes { get; set; }
    }
}
