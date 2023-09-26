using Locompro.Models;
using Locompro.Repositories;
using Locompro.Services;
using System.Xml;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddDbContext<LocomproContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LocomproContext") ?? throw new InvalidOperationException("Connection string 'LocomproContext' not found.")));

// Add DbContext using SQL Server
//builder.Services.AddDbContext<laboratorio4Context>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("laboratorio4Context") ?? throw new InvalidOperationException("Connection string 'laboratorio4Context' not found.")));

// Register repositories and services
builder.Services.AddScoped<UnitOfWork>();
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

