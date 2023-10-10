﻿using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Services.Domain
{
    /// <summary>
    /// Domain service for Store entities.
    /// </summary>
    public class StoreService : StringIdDomainService<Store, StoreRepository>
    {
        /// <summary>
        /// Constructs a Store service for a given repository.
        /// </summary>
        /// <param name="unitOfWork">Unit of work to handle transactions.</param>
        /// <param name="repository">Repository to base the service on.</param>
        /// <param name="loggerFactory">Factory for service logger.</param>
        public StoreService(UnitOfWork unitOfWork, StoreRepository repository, ILoggerFactory loggerFactory) 
            : base(unitOfWork, repository, loggerFactory)
        {
        }
    }
}
