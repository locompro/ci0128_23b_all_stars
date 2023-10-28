using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Locompro.Data;
using Locompro.Repositories;
using Locompro.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Server;
using System.Xml;
using Locompro.Models;
using Locompro.Services.Domain;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

// Register repositories and services
RegisterServices(webApplicationBuilder);

var app = webApplicationBuilder.Build();

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

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseHttpLogging();

app.UseAuthorization();

app.MapRazorPages();

app.Use(async (context, next) =>
{
    Console.WriteLine($"Incoming request: {context.Request.Path}");
    await next();
});

app.Run();
return;

void RegisterServices(WebApplicationBuilder builder)
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    // Built in services
    builder.Services.AddLogging();
    builder.Services.AddRazorPages();
    builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();

    // Add DbContext using SQL Server
    builder.Services.AddDbContext<LocomproContext>(options =>
        options.UseLazyLoadingProxies()
            .UseSqlServer(builder.Configuration.GetConnectionString("LocomproContext") ??
                          throw new InvalidOperationException("Connection string 'LocomproContext' not found.")));

    // Set LocomproContext as the default DbContext
    builder.Services.AddScoped<DbContext, LocomproContext>();

    builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<LocomproContext>();

    // Configure application cookie after setting up Identity
    builder.Services.ConfigureApplicationCookie(config =>
    {
        config.Cookie.Name = "Identity.Cookie";
        config.LoginPath = "/Account/Login";
    });

    // Register repositories
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddScoped<StoreRepository>();
    builder.Services.AddScoped<CountryRepository>();
    builder.Services.AddScoped<CantonRepository>();
    builder.Services.AddScoped<CategoryRepository>();
    builder.Services.AddScoped<ProductRepository>();
    builder.Services.AddScoped<SubmissionRepository>();
    builder.Services.AddScoped<UserRepository>();

    // Register domain services
    builder.Services.AddScoped<StoreService>();
    builder.Services.AddScoped<CountryService>();
    builder.Services.AddScoped<CantonService>();
    builder.Services.AddScoped<CategoryService>();
    builder.Services.AddScoped<ProductService>();
    builder.Services.AddScoped<SubmissionService>();
    builder.Services.AddScoped<UserService>();

    // Register application services
    builder.Services.AddScoped<ContributionService>();
    builder.Services.AddScoped<AuthService>();
    builder.Services.AddScoped<AdvancedSearchInputService>();
    builder.Services.AddScoped<SearchService>();
}