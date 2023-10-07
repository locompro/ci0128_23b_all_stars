using Locompro.Services;
using Locompro.Models;
using Locompro.Repositories;
using Microsoft.Extensions.Logging;


namespace Locompro.Services
{
    /// <summary>
    /// Service for Category
    /// </summary>
    public class CategoryService : AbstractDomainService<Category, string, CategoryRepository>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="repository"></param>
        /// <param name="loggerFactory"></param>
        public CategoryService(UnitOfWork unitOfWork, CategoryRepository repository, ILoggerFactory loggerFactory)
            : base(unitOfWork, repository, loggerFactory)
        {
        }
    }
}
