using Locompro.Data;
using Locompro.Repositories;

namespace Locompro.Services.Domain
{
    /// <summary>
    /// Generic domain service for an application entity type.
    /// </summary>
    /// <typeparam name="T">Type of entity handled by service.</typeparam>
    /// <typeparam name="I">Type of key used by entity.</typeparam>
    /// <typeparam name="R">Type of repository used by service.</typeparam>
    public abstract class AbstractDomainService<T, I, R> : AbstractService, IDomainService<T, I>
        where T : class
        where R : IRepository<T, I>
    {
        protected readonly R Repository;

        /// <summary>
        /// Constructs a domain service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="repository">Repository to base the service on.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        protected AbstractDomainService(UnitOfWork unitOfWork, R repository, ILoggerFactory loggerFactory) 
            : base(unitOfWork, loggerFactory)
        {
            this.Repository = repository;
        }

        /// <inheritdoc />
        public async Task<T> Get(I id)
        {
            await UnitOfWork.BeginTransaction();

            try
            {
                return await Repository.GetByIdAsync(id);
            }
            catch (Exception)
            {
                await UnitOfWork.Rollback();
                throw;
            }
            finally
            {
                await UnitOfWork.Commit();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAll()
        {
            await UnitOfWork.BeginTransaction();

            try
            {
                return await Repository.GetAllAsync();
            }
            catch (Exception)
            {
                await UnitOfWork.Rollback();
                throw;
            }
            finally
            {
                await UnitOfWork.Commit();
            }
        }

        /// <inheritdoc />
        public async Task Add(T entity)
        {
            await UnitOfWork.BeginTransaction();

            try
            {
                await Repository.AddAsync(entity);
            }
            catch (Exception)
            {
                await UnitOfWork.Rollback();
                throw;
            }
            finally
            {
                await UnitOfWork.Commit();
            }
        }

        /// <inheritdoc />
        public async Task Update(T entity)
        {
            await UnitOfWork.BeginTransaction();

            try
            {
                await Repository.UpdateAsync(entity);
            }
            catch (Exception)
            {
                await UnitOfWork.Rollback();
                throw;
            }
            finally
            {
                await UnitOfWork.Commit();
            }
        }

        /// <inheritdoc />
        public async Task Delete(I id)
        {
            await UnitOfWork.BeginTransaction();

            try
            {
                await Repository.DeleteAsync(id);
            }
            catch (Exception)
            {
                await UnitOfWork.Rollback();
                throw;
            }
            finally
            {
                await UnitOfWork.Commit();
            }
        }
    }
}