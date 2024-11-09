using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ReviewerProject.Models
{
    public class ApplicationContext:IdentityDbContext<User>
    {
        public DbSet<ReviewingType> ReviewingTypes { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
    }
}
