using System;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Locompro.Data;
using Locompro.Data.Repositories;
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

    string environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                             throw new InvalidOperationException("Env environment variable not found.");
    
    var connectionString = builder.Configuration.GetValue<string>($"{environmentName}_ConnectionString__LocomproContext") ??
                           throw new InvalidOperationException("Secret connection string not found.");
    
    // Add DbContext using SQL Server
    builder.Services.AddDbContext<LocomproContext>(options => options.UseLazyLoadingProxies()
        .UseSqlServer(connectionString));

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
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    builder.Services.AddScoped(typeof(ICrudRepository<,>), typeof(CrudRepository<,>));
    builder.Services.AddScoped(typeof(INamedEntityRepository<,>), typeof(NamedEntityRepository<,>));
    builder.Services.AddScoped<ISubmissionRepository, SubmissionRepository>();
    builder.Services.AddScoped<ICantonRepository, CantonRepository>();
    builder.Services.AddScoped<ProductRepository>();

    // Register domain services
    builder.Services.AddScoped(typeof(INamedEntityDomainService<,>), typeof(NamedEntityDomainService<,>));
    builder.Services.AddScoped(typeof(IDomainService<,>), typeof(DomainService<,>));
    builder.Services.AddScoped<ISubmissionService, SubmissionService>();
    builder.Services.AddScoped<ICantonService, CantonService>();
    builder.Services.AddScoped<ProductService>();
    builder.Services.AddScoped<UserService>();

    // Register application services
    builder.Services.AddScoped<IContributionService, ContributionService>();
    builder.Services.AddScoped<AuthService>();
    builder.Services.AddScoped<AdvancedSearchInputService>();
    builder.Services.AddScoped<ISearchDomainService, SearchDomainService>();
    builder.Services.AddScoped<ISearchService, SearchService>();
}