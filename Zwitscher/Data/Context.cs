
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Zwitscher.Models;

namespace Zwitscher.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts)
            : base(opts)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                   .HasIndex(u => u.UserName)
                   .IsUnique();
        }
    }

}
