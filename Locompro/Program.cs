using Microsoft.EntityFrameworkCore;

using Locompro.Data;
using Locompro.Repositories;
using Locompro.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Components.Server;
using System.Xml;


var builder = WebApplication.CreateBuilder(args);

registerServices(builder);

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
app.UseAuthentication();;

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
    builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false) /* add services to user here*/
        .AddEntityFrameworkStores<LocomproContext>();

    // Register repositories and services
    builder.Services.AddScoped<UnitOfWork>();
    builder.Services.AddScoped<StoreRepository>();
    builder.Services.AddScoped<StoreService>();
    builder.Services.AddScoped<AuthService>();
}


