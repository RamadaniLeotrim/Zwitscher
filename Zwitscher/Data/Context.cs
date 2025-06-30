
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

        public DbSet<ChatMessage> ChatMessages => Set<ChatMessage>();

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                   .HasIndex(u => u.UserName)
                   .IsUnique();

            builder.Entity<ChatMessage>()
                   .HasOne(cm => cm.User)
                   .WithMany() // falls User.NavigationCollection nicht existiert
                   .HasForeignKey(cm => cm.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }

}
