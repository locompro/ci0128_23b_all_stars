using System.Collections.Concurrent;
using Locompro.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Locompro.Data;

/// <summary>
///     DB transaction handler.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly DbContext _context;
    private readonly ILogger _logger;

    private readonly ILoggerFactory _loggerFactory;
    private readonly ConcurrentDictionary<Type, IRepository> _repositories;
    private readonly Dictionary<Type, Func<IRepository>> _specialRepositoryFactories;

    /// <summary>
    ///     Constructs a unit of work for a given context.
    /// </summary>
    /// <param name="loggerFactory">Logger for unit of work.</param>
    /// <param name="context"></param>
    public UnitOfWork(ILoggerFactory loggerFactory, DbContext context)
    {
        _logger = loggerFactory.CreateLogger(GetType());
        _context = context;

        _loggerFactory = loggerFactory;
        _repositories = new ConcurrentDictionary<Type, IRepository>();
        _specialRepositoryFactories = new Dictionary<Type, Func<IRepository>>
        {
            { typeof(ICantonRepository), () => new CantonRepository(_context, _loggerFactory) },
            { typeof(ISubmissionRepository), () => new SubmissionRepository(_context, _loggerFactory) },
            { typeof(IPictureRepository), () => new PictureRepository(_context, _loggerFactory) },
            { typeof(IUserRepository), () => new UserRepository(_context, _loggerFactory) },
            { typeof(IReportRepository), () => new ReportRepository(_context, _loggerFactory) }
        };
    }

    public async Task SaveChangesAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
            _logger.LogInformation("Changes saved successfully");
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Failed to save changes");
            throw;
        }
    }

    public void RegisterRepository<TR>(TR repository) where TR : class, IRepository
    {
        var type = typeof(TR);
        _repositories[type] = repository;
    }

    public ICrudRepository<T, TK> GetCrudRepository<T, TK>() where T : class
    {
        var type = typeof(ICrudRepository<T, TK>);
        if (_repositories.TryGetValue(type, out var repository)) return (ICrudRepository<T, TK>)repository;
        repository = new CrudRepository<T, TK>(_context, _loggerFactory);
        _repositories[type] = repository;
        return (ICrudRepository<T, TK>)repository;
    }

    public INamedEntityRepository<T, TK> GetNamedEntityRepository<T, TK>() where T : class
    {
        var type = typeof(INamedEntityRepository<T, TK>);
        if (_repositories.TryGetValue(type, out var repository)) return (INamedEntityRepository<T, TK>)repository;
        repository = new NamedEntityRepository<T, TK>(_context, _loggerFactory);
        _repositories[type] = repository;
        return (INamedEntityRepository<T, TK>)repository;
    }

    public TR GetSpecialRepository<TR>() where TR : class, IRepository
    {
        var type = typeof(TR);
        if (_repositories.TryGetValue(type, out var repository)) return (TR)repository;
        if (_specialRepositoryFactories.TryGetValue(typeof(TR), out var factory))
        {
            repository = factory();
            _repositories[type] = repository;
            return (TR)repository;
        }

        throw new ArgumentException($"{type} is not a valid repository type.");
    }
}