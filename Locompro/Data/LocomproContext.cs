using Locompro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data
{
    public class LocomproContext : IdentityDbContext<User>
    {
        public LocomproContext(DbContextOptions<LocomproContext> options)
            : base(options)
        {
        }

        public DbSet<Locompro.Models.Store> Store { get; set; } = default!;
        public DbSet<Locompro.Models.Province> Province { get; set; } = default!;
        public DbSet<Locompro.Models.Country> Country { get; set; } = default!;
        public DbSet<Locompro.Models.Canton> Canton { get; set; } = default!;
    }
}
