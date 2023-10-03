﻿using Locompro.Services;
using Locompro.Models;
using Locompro.Repositories;


namespace Locompro.Services
{
    public class CategoryService : AbstractDomainService<Category, string, CategoryRepository>
    {
        public CategoryService(UnitOfWork unitOfWork, CategoryRepository repository, ILoggerFactory loggerFactory)
            : base(unitOfWork, repository, loggerFactory)
        {
        }
    }
}