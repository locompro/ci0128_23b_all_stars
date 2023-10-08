﻿using Locompro.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data;

public class LocomproContext : IdentityDbContext<User>
{
    public DbSet<Country> Countries { get; set; } = default!;
    public DbSet<Province> Provinces { get; set; } = default!;
    public DbSet<Canton> Cantons { get; set; } = default!;
    public DbSet<Category> Categories { get; set; } = default!;
    public DbSet<Submission> Submissions { get; set; } = default!;
    public DbSet<Store> Stores { get; set; } = default!;
    public DbSet<Product> Products { get; set; } = default!;

    public LocomproContext(DbContextOptions<LocomproContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<Province>()
            .HasKey(p => new { p.CountryName, p.Name });

        builder.Entity<Province>()
            .HasOne(p => p.Country)
            .WithMany(c => c.Provinces)
            .HasForeignKey(p => p.CountryName)
            .IsRequired();
        
        builder.Entity<Canton>()
            .HasKey(c => new { c.CountryName, c.ProvinceName, c.Name });

        builder.Entity<Canton>()
            .HasOne(c => c.Province)
            .WithMany(p => p.Cantons)
            .HasForeignKey(c => new { c.CountryName, c.ProvinceName })
            .IsRequired();

        builder.Entity<Category>()
            .HasKey(c => new { c.Name });
        
        builder.Entity<Category>()
            .HasOne(c => c.Parent)
            .WithMany(c => c.Children);

        builder.Entity<Product>()
            .HasMany(p => p.Categories)
            .WithMany(c => c.Products);
        
        builder.Entity<Submission>()
            .HasKey(s => new { s.Username, s.EntryTime });

        builder.Entity<Submission>()
            .HasOne(s => s.Store)
            .WithMany()
            .HasForeignKey(s => s.StoreName)
            .IsRequired();
        
        builder.Entity<Submission>()
            .HasOne(s => s.Product)
            .WithMany(p => p.Submissions)
            .HasForeignKey(s => s.ProductId)
            .IsRequired();
        
        builder.Entity<Submission>()
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.Username)
            .IsRequired();

        builder.Entity<User>()
            .HasMany(u => u.Submissions)
            .WithOne(s => s.User);
    }
}