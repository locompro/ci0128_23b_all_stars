using Microsoft.EntityFrameworkCore;

using Locompro.Data;
using Locompro.Repositories;
using Locompro.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Server;
using System.Xml;
using Locompro.Models;


var builder = WebApplication.CreateBuilder(args);

registerServices(builder);

//    options.UseSqlServer(builder.Configuration.GetConnectionString("laboratorio4Context") ?? throw new InvalidOperationException("Connection string 'laboratorio4Context' not found.")));

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
app.UseAuthentication();;

//app.UseHttpLogging();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

void registerServices(WebApplicationBuilder builder)
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    // Add services to the container.
    builder.Services.AddRazorPages();
    builder.Services.AddScoped<UnitOfWork>();

    // Register repositories and services
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddScoped<StoreRepository>();
    builder.Services.AddScoped<StoreService>();
    
    builder.Services.AddTransient<CountryRepository>();
    builder.Services.AddTransient<CountryService>();
    
    // Add DbContext using SQL Server
    builder.Services.AddDbContext<LocomproContext>(options =>
        options.UseLazyLoadingProxies()
            .UseSqlServer(builder.Configuration.GetConnectionString("LocomproContext") ?? throw new InvalidOperationException("Connection string 'LocomproContext' not found.")));
    builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<LocomproContext>();

    // Register repositories and services
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddScoped<StoreRepository>();
    builder.Services.AddScoped<StoreService>();

    // for advanced search
    builder.Services.AddScoped<AdvancedSearchModalService>();
    builder.Services.AddScoped<CategoryRepository>();
    builder.Services.AddScoped<CategoryService>();
}


