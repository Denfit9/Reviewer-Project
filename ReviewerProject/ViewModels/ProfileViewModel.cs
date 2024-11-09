using ReviewerProject.Models;
using System.Reflection;

namespace ReviewerProject.ViewModels
{
    public class ProfileViewModel: User
    {
        public User _user { get; set; }
        public double Average { get; set; }
        public string SearchResult {  get; set; }
        public ProfileViewModel(User user, string searchResult)
        {
            _user = user;
            SearchResult = searchResult;
            foreach (PropertyInfo prop in user.GetType().GetProperties())
                prop.SetValue(this, prop.GetValue(user));
            if (this.Reviews != null)
            {
                double ratingAverage = 0;
                foreach (var review in this.Reviews)
                {
                    ratingAverage += review.Rating;
                }
                this.Average = ratingAverage/this.Reviews.Count;
            }
        }
    }
}
