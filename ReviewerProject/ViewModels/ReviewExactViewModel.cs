using ReviewerProject.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewerProject.ViewModels
{
    public class ReviewExactViewModel: ReviewBase
    {
        public Guid ReviewingTypeKey { get; set; }
        public ReviewingType ReviewingType { get; set; }
        public string UserKey { get; set; }

        public User User { get; set; }
    }
}
