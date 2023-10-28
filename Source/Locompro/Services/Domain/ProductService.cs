﻿using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain
{
    /// <summary>
    /// Domain service for Product entities.
    /// </summary>
    public class ProductService : DomainService<Product, int>
    {
        private readonly INamedEntityRepository<Product, int> _namedEntityRepository;

        /// <summary>
        /// Constructs a Product service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        public ProductService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
        {
            _namedEntityRepository = UnitOfWork.GetRepository<INamedEntityRepository<Product, int>>();
        }

        public async Task<IEnumerable<Product>> GetByPartialName(string partialName)
        {
            return await _namedEntityRepository.GetByPartialNameAsync(partialName);
        }
    }
}