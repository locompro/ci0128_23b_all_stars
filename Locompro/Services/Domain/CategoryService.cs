using Locompro.Data;
using Locompro.Models;
using Locompro.Repositories;

namespace Locompro.Services.Domain
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
