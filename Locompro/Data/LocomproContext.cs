using Locompro.Models;

using Microsoft.EntityFrameworkCore;

namespace Locompro.Data
{
    public class LocomproContext : DbContext
    {
        public LocomproContext(DbContextOptions<LocomproContext> options)
            : base(options)
        {
        }

        public DbSet<Locompro.Models.Store> Store { get; set; } = default!;
    }
}
