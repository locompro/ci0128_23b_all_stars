using System;
using Microsoft.EntityFrameworkCore;

using Locompro.Data;
using Locompro.Repositories;
using Locompro.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Server;
using System.Xml;
using Locompro.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


var builder = WebApplication.CreateBuilder(args);

registerServices(builder);

// Register repositories and services
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
    app.UseMigrationsEndPoint();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<LocomproContext>();

    // TODO: delete before merge
    context.Database.EnsureDeleted();
    
    if (context.Database.EnsureCreated())
    {
        Console.WriteLine("Database created");
    } else
    {
        Console.WriteLine("Database already exists");
    }

    SeedData.Initialize(context);
}


app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseHttpLogging();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

void registerServices(WebApplicationBuilder builder)
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    // Add services to the container.
    builder.Services.AddLogging();
    builder.Services.AddRazorPages();
    builder.Services.AddScoped<UnitOfWork>();

    // Register repositories and services
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddScoped<StoreRepository>();
    builder.Services.AddScoped<StoreService>();
    builder.Services.AddScoped<AuthService>();
    
    builder.Services.AddTransient<CountryRepository>();
    builder.Services.AddTransient<CountryService>();
    
    // Add DbContext using SQL Server
    builder.Services.AddDbContext<LocomproContext>(options =>
        options.UseLazyLoadingProxies()
            .UseSqlServer(builder.Configuration.GetConnectionString("LocomproContext") ?? throw new InvalidOperationException("Connection string 'LocomproContext' not found.")));
    builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<LocomproContext>();

    // Register repositories and services
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddScoped<StoreRepository>();
    builder.Services.AddScoped<StoreService>();

    // for advanced search
    builder.Services.AddScoped<AdvancedSearchModalService>();
    builder.Services.AddScoped<SearchService>();
    builder.Services.AddScoped<CategoryRepository>();
    builder.Services.AddScoped<CategoryService>();
    builder.Services.AddScoped<ProductRepository>();
    builder.Services.AddScoped<ProductService>();
    builder.Services.AddScoped<SubmissionRepository>();
}


