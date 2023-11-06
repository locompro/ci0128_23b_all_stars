using Locompro.Common;
using Locompro.Common.ErrorStore;
using Microsoft.EntityFrameworkCore;
using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Services;
using Locompro.Models;
using Locompro.Models.Entities;
using Locompro.Services.Auth;
using Locompro.Services.Domain;
using Locompro.Services.Tasks;

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

app.UseSession();

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

    var connectionString =
        builder.Configuration.GetValue<string>($"{environmentName}_ConnectionString__LocomproContext") ??
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
    builder.Services.AddScoped<IPictureRepository, PictureRepository>();
    builder.Services.AddScoped<IUserRepository, UserRepository>();

    // Register domain services
    builder.Services.AddScoped(typeof(INamedEntityDomainService<,>), typeof(NamedEntityDomainService<,>));
    builder.Services.AddScoped(typeof(IDomainService<,>), typeof(DomainService<,>));
    builder.Services.AddScoped<ISubmissionService, SubmissionService>();
    builder.Services.AddScoped<ICantonService, CantonService>();
    builder.Services.AddScoped<ISignInManagerService, SignInManagerService>();
    builder.Services.AddScoped<IUserManagerService, UserManagerService>();
    builder.Services.AddScoped<IUserService, Locompro.Services.Domain.UserService>();

    // Register application services
    builder.Services.AddScoped<IContributionService, ContributionService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IErrorStore, ErrorStore>();
    builder.Services.AddScoped<AdvancedSearchInputService>();
    builder.Services.AddScoped<ISearchDomainService, SearchDomainService>();
    builder.Services.AddScoped<ISearchService, SearchService>();
    builder.Services.AddScoped<IPictureService, PictureService>();
    builder.Services.AddScoped<SearchService>();
    builder.Services.AddScoped<IModerationService, ModerationService>();


    builder.Services.AddSingleton<IErrorStoreFactory, ErrorStoreFactory>();
    
    // Add session support
    builder.Services.AddSession(options =>
    {
        options.IdleTimeout = TimeSpan.FromMinutes(5);
    });
    
    RegisterHostedServices(builder);
}

// Register each related task next to each other. At the end of the related tasks, register the hosted service.
// The Hosted Service will run all the tasks in the order they were registered.
void RegisterHostedServices(WebApplicationBuilder builder)
{
    // Moderation tasks
    builder.Services.AddSingleton<IScheduledTask, AddPossibleModeratorsTask>();
    builder.Services.AddHostedService<TaskSchedulerService>();
    
}