using Locompro.Data;

namespace Locompro.Services;

/// <summary>
///     Generic application service.
/// </summary>
public abstract class Service
{
    protected readonly ILogger Logger;

    /// <summary>
    ///     Constructs a service.
    /// </summary>
    /// <param name="loggerFactory">Factory for service logger.</param>
    protected Service(ILoggerFactory loggerFactory)
    {
        Logger = loggerFactory.CreateLogger(GetType());
    }
}