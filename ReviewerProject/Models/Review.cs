using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewerProject.Models
{
    public class Review: ReviewBase
    {

        public Guid ReviewingTypeKey { get; set; }
        [ForeignKey("ReviewingTypeKey")]
        public ReviewingType ReviewingType { get; set; }

        public string UserKey { get; set; }
        [ForeignKey("UserKey")]
        public User User { get; set; }
    }
}
