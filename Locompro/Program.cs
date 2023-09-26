using Microsoft.EntityFrameworkCore;

using Locompro.Data;
using Locompro.Repositories;
using Locompro.Services;

var builder = WebApplication.CreateBuilder(args);

registerServices(builder);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();

void registerServices(WebApplicationBuilder builder)
{
    // Add services to the container.
    builder.Services.AddRazorPages();

    // Add DbContext using SQL Server
    builder.Services.AddDbContext<LocomproContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("LocomproContext") ?? throw new InvalidOperationException("Connection string 'LocomproContext' not found.")));

    // Register repositories and services
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddScoped<StoreRepository>();
    builder.Services.AddScoped<StoreService>();
}