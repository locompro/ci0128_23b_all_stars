using Microsoft.EntityFrameworkCore;

namespace Locompro.Data
{
    public class LocomproContext : DbContext
    {
        public LocomproContext (DbContextOptions<LocomproContext> options)
            : base(options)
        {
        }

        public DbSet<Models.User> User { get; set; } = default!;
        public DbSet<Models.UserRole> UserRole { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Models.User>().ToTable("User");
            modelBuilder.Entity<Models.UserRole>().ToTable("UserRole");
            // Define the composite primary key for UserRole
            modelBuilder.Entity<Models.UserRole>()
                .HasKey(ur => new { ur.Username, ur.Role });
        }
    }
}
