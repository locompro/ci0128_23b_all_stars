namespace Locompro.Services.Tasks;

public class FindPriceAnomaliesTask : ScheduledTaskBase<ModerationService>
{
    public FindPriceAnomaliesTask(IServiceProvider serviceProvider) : base(serviceProvider)
    {
    }

    protected override Task ExecuteScopedAsync(CancellationToken cancellationToken)
    {
        // Busca las submissions anómalas
        
        return Task.CompletedTask;
    }
}