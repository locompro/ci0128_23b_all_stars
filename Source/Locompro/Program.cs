using Locompro.Common;
using Locompro.Common.ErrorStore;
using Locompro.Data;
using Locompro.Data.Repositories;
using Locompro.Models.Entities;
using Locompro.Services;
using Locompro.Services.Auth;
using Locompro.Services.Domain;
using Locompro.Services.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

var webApplicationBuilder = WebApplication.CreateBuilder(args);

AddConfigurationFileToBuilder(webApplicationBuilder);
// Register repositories and services
RegisterServices(webApplicationBuilder);
// Configure logging
ConfigureLogging(webApplicationBuilder);

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
    AddDatabaseServices(builder);
    // Configure application cookie after setting up Identity
    builder.Services.ConfigureApplicationCookie(config =>
    {
        config.Cookie.Name = "Identity.Cookie";
        config.LoginPath = "/Account/Login";
    });
    
    // Built in services
    builder.Services.AddLogging();
    builder.Services.AddRazorPages();
    builder.Services.AddSingleton<ILoggerFactory, LoggerFactory>();
    // Register Unit of Work
    builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
    // Register domain services
    builder.Services.AddScoped(typeof(INamedEntityDomainService<,>), typeof(NamedEntityDomainService<,>));
    builder.Services.AddScoped(typeof(IDomainService<,>), typeof(DomainService<,>));
    builder.Services.AddScoped<ISubmissionService, SubmissionService>();
    builder.Services.AddScoped<ICantonService, CantonService>();
    builder.Services.AddScoped<ISignInManagerService, SignInManagerService>();
    builder.Services.AddScoped<IUserManagerService, UserManagerService>();
    builder.Services.AddScoped<IUserService, UserService>();
    builder.Services.AddScoped<IReportService, ReportService>();
    builder.Services.AddScoped<IProductService, ProductService>();

    // Register application services
    builder.Services.AddScoped<IContributionService, ContributionService>();
    builder.Services.AddScoped<IAuthService, AuthService>();
    builder.Services.AddScoped<IErrorStore, ErrorStore>();
    builder.Services.AddScoped<AdvancedSearchInputService>();
    builder.Services.AddScoped<ISearchService, SearchService>();
    builder.Services.AddScoped<IPictureService, PictureService>();
    builder.Services.AddScoped<SearchService>();
    builder.Services.AddScoped<IModerationService, ModerationService>();


    builder.Services.AddSingleton<IErrorStoreFactory, ErrorStoreFactory>();
    builder.Services.AddSingleton<IApiKeyHandler>(serviceProvider => {
        var configuration = serviceProvider.GetRequiredService<IConfiguration>();
        var googleApiKey = configuration["ApiKeys:Google"];
        return new ApiKeyHandler(googleApiKey);
    });

    // Add session support
    builder.Services.AddSession(options => { options.IdleTimeout = TimeSpan.FromMinutes(5); });

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

void AddDatabaseServices(WebApplicationBuilder builder)
{
    builder.Services.AddDatabaseDeveloperPageExceptionFilter();

    try
    {
        var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ??
                              throw new InvalidOperationException("Env environment variable not found.");

        string connectionString = builder.Configuration["ConnectionString:Development"];

        if (environmentName == "Production")
        {
            connectionString = builder.Configuration["ConnectionString:Production"];
        }

        builder.Services.AddDbContext<LocomproContext>(options =>
        {
            if (connectionString != null)
                options.UseLazyLoadingProxies()
                    .UseSqlServer(connectionString);
        });
    }
    catch (InvalidOperationException e)
    {
        Console.WriteLine(e);
        Environment.Exit(1);
    }

    // Set LocomproContext as the default DbContext
    builder.Services.AddScoped<DbContext, LocomproContext>();

    builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
        .AddEntityFrameworkStores<LocomproContext>();
}

void AddConfigurationFileToBuilder(WebApplicationBuilder builder)
{

    var configurationFilePath = "secrets.json";

    try
    {
        builder.Configuration.AddJsonFile(configurationFilePath, optional: false, reloadOnChange: true);
        
    }
    catch (Exception e)
    {
        Console.WriteLine(e);
        Console.WriteLine($"Error loading configuration file: {configurationFilePath}");
        Environment.Exit(1);
    }
}

void ConfigureLogging(WebApplicationBuilder builder)
{
    builder.Logging.ClearProviders(); // If you want to remove default providers
    builder.Logging.AddConsole();
    builder.Logging.AddDebug();
    builder.Logging.AddEventSourceLogger();
    // Configure log level for Microsoft.EntityFrameworkCore.Database.Command
    builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Database.Command", LogLevel.Warning);
    builder.Logging.AddFilter("Microsoft.EntityFrameworkCore.Infrastructure", LogLevel.Warning);
}
