using Microsoft.AspNetCore.Identity;
using ReviewerProject.ViewModels;

namespace ReviewerProject.Models
{
    public class User: IdentityUser
    {
        public string FirstName { get; set; }
        public string? LastName { get; set; }
        public List<ReviewViewModel> Reviews { get; set; }
    }
}
