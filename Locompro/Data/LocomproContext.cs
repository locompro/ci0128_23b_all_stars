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
    }
}
