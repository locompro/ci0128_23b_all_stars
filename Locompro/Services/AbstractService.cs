﻿using Locompro.Repositories;
namespace Locompro.Services
{
    /// <summary>
    /// Abstract class representing application services.
    /// </summary>
    /// <typeparam name="T">Type of entity handled by service.</typeparam>
    /// <typeparam name="I">Type of key used by entity.</typeparam>
    /// <typeparam name="R">Type of repository used by service.</typeparam>
    public abstract class AbstractService<T, I, R> : IService<T, I>
        where T : class
        where R: IRepository<T, I>
    {
        protected readonly UnitOfWork unitOfWork;
        protected readonly R repository;

        protected AbstractService(UnitOfWork unitOfWork, R repository)
        {
            this.unitOfWork = unitOfWork;
            this.repository = repository;
        }

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
        }

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
        }

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
        }

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
        }

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
        }
    }
}
