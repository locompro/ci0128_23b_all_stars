using Locompro.Data;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain
{
    /// <summary>
    /// Generic domain service for an application entity type.
    /// </summary>
    /// <typeparam name="T">Type of entity handled by service.</typeparam>
    /// <typeparam name="I">Type of key used by entity.</typeparam>
    public class DomainService<T, I> : Service, IDomainService<T, I>
        where T : class
    {
        protected readonly ICrudRepository<T, I> CrudRepository;

        /// <summary>
        /// Constructs a domain service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        public DomainService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory)
            : base(unitOfWork, loggerFactory)
        {
            this.CrudRepository = UnitOfWork.GetRepository<T, I>();
        }

        /// <inheritdoc />
        public async Task<T> Get(I id)
        {
            return await CrudRepository.GetByIdAsync(id);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAll()
        {
            return await CrudRepository.GetAllAsync();
        }

        /// <inheritdoc />
        public async Task Add(T entity)
        {
            await UnitOfWork.BeginTransactionAsync();

            try
            {
                await CrudRepository.AddAsync(entity);
                await UnitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to add entity");
                await UnitOfWork.RollbackAsync();
                throw;
            }
        }

        /// <inheritdoc />
        public async Task Update(T entity)
        {
            await UnitOfWork.BeginTransactionAsync();

            try
            {
                await CrudRepository.UpdateAsync(entity);
                await UnitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to update entity");
                await UnitOfWork.RollbackAsync();
                throw;
            }
        }

        /// <inheritdoc />
        public async Task Delete(I id)
        {
            await UnitOfWork.BeginTransactionAsync();

            try
            {
                await CrudRepository.DeleteAsync(id);
                await UnitOfWork.CommitAsync();
            }
            catch (Exception e)
            {
                Logger.LogError(e, "Failed to delete entity");
                await UnitOfWork.RollbackAsync();
                throw;
            }
        }
    }
}