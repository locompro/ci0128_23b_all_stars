﻿using Locompro.Common.Search;
using Locompro.Common.Search.QueryBuilder;
using Locompro.Common.Search.SearchMethodRegistration.SearchMethods;
using Locompro.Data;
using Locompro.Data.Repositories;
using Microsoft.AspNetCore.Identity.UI.V4.Pages.Account.Manage.Internal;

namespace Locompro.Services.Domain;

/// <summary>
///     Generic domain service for an application entity type.
/// </summary>
/// <typeparam name="T">Type of entity handled by service.</typeparam>
/// <typeparam name="TK">Type of key used by entity.</typeparam>
public class DomainService<T, TK> : Service, IDomainService<T, TK>
    where T : class
{
    protected readonly IUnitOfWork UnitOfWork;
    protected readonly ICrudRepository<T, TK> CrudRepository;
    protected readonly IQueryBuilder<T> QueryBuilder;

    /// <summary>
    ///     Constructs a domain service for a given repository.
    /// </summary>
    /// <param name="unitOfWork">Unit of work to handle transactions.</param>
    /// <param name="loggerFactory">Factory for service logger.</param>
    public DomainService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
        : base(loggerFactory)
    {
        UnitOfWork = unitOfWork;
        CrudRepository = UnitOfWork.GetCrudRepository<T, TK>();
        QueryBuilder = new QueryBuilder<T>(SearchMethodsFactory.GetInstance().Create<T>(), Logger);
    }

    /// <inheritdoc />
    public async Task<T> Get(TK id)
    {
        return await CrudRepository.GetByIdAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetAll()
    {
        return await CrudRepository.GetAllAsync();
    }
    
    /// <inheritdoc />
    public async Task<IEnumerable<T>> GetByDynamicQuery(ISearchQueryParameters<T> searchQueries)
    {
        QueryBuilder.AddSearchCriteria(searchQueries);
        
        ISearchQueries<T> builtQueries = QueryBuilder.GetSearchFunction();
        
        IEnumerable<T> results = builtQueries.IsEmpty()?
            null :
            await CrudRepository.GetByDynamicQuery(builtQueries);
        
        QueryBuilder.Reset();
        
        return results;
    }

    /// <inheritdoc />
    public async Task Add(T entity)
    {
        try
        {
            await CrudRepository.AddAsync(entity);
            await UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to add entity");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task Update(TK id, T entity)
    {
        try
        {
            await CrudRepository.UpdateAsync(id, entity);
            await UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to update entity");
            throw;
        }
    }

    /// <inheritdoc />
    public async Task Delete(TK id)
    {
        try
        {
            await CrudRepository.DeleteAsync(id);
            await UnitOfWork.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Logger.LogError(e, "Failed to delete entity");
            throw;
        }
    }
}