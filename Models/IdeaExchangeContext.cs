using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdeaExchange.Models
{
    public class IdeaExchangeContext : IdentityDbContext<ApplicationUser>
    {

        public IdeaExchangeContext(DbContextOptions<IdeaExchangeContext> options) : base(options)
        {

        }
        public DbSet<Publication> Publications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Publication>()
                .HasOne(p => p.Author)
                .WithMany()
                .HasForeignKey(p => p.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

}


