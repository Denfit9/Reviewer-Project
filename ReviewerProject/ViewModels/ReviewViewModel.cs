using ReviewerProject.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReviewerProject.ViewModels
{
    public class ReviewViewModel: ReviewBase
    {
        public string ReviewingType { get; set; }
        public string UserKey { get; set; }
    }
}
