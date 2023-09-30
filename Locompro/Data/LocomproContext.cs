using Locompro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data;

public class LocomproContext : IdentityDbContext<User>
{
    public LocomproContext(DbContextOptions<LocomproContext> options)
        : base(options)
    {
    }

    public DbSet<Store> Store { get; set; } = default!;
    public DbSet<Province> Province { get; set; } = default!;
    public DbSet<Country> Country { get; set; } = default!;
    public DbSet<Canton> Canton { get; set; } = default!;
}