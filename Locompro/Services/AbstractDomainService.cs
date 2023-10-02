using Locompro.Repositories;
namespace Locompro.Services
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
        protected readonly R repository;

        /// <summary>
        /// Constructs a domain service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="repository">Repository to base the service on.</param>
        protected AbstractDomainService(UnitOfWork unitOfWork, R repository) : base(unitOfWork)
        {
            this.repository = repository;
        }

        /// <inheritdoc />
        public async Task<T> Get(I id)
        {
            await unitOfWork.BeginTransaction();

            try
            {
                return await repository.GetByIdAsync(id);
            }
            catch (Exception)
            {
                await unitOfWork.Rollback();
                throw;
            }
            finally
            {
                await unitOfWork.Commit();
            }
        }

        /// <inheritdoc />
        public async Task<IEnumerable<T>> GetAll()
        {
            await unitOfWork.BeginTransaction();

            try
            {
                return await repository.GetAllAsync();
            }
            catch (Exception)
            {
                await unitOfWork.Rollback();
                throw;
            }
            finally
            {
                await unitOfWork.Commit();
            }
        }

        /// <inheritdoc />
        public async Task Add(T entity)
        {
            await unitOfWork.BeginTransaction();

            try
            {
                await repository.AddAsync(entity);
            }
            catch (Exception)
            {
                await unitOfWork.Rollback();
                throw;
            }
            finally
            {
                await unitOfWork.Commit();
            }
        }

        /// <inheritdoc />
        public async Task Update(T entity)
        {
            await unitOfWork.BeginTransaction();

            try
            {
                await repository.UpdateAsync(entity);
            }
            catch (Exception)
            {
                await unitOfWork.Rollback();
                throw;
            }
            finally
            {
                await unitOfWork.Commit();
            }
        }

        /// <inheritdoc />
        public async Task Delete(I id)
        {
            await unitOfWork.BeginTransaction();

            try
            {
                await repository.DeleteAsync(id);
            }
            catch (Exception)
            {
                await unitOfWork.Rollback();
                throw;
            }
            finally
            {
                await unitOfWork.Commit();
            }
        }
    }
}
