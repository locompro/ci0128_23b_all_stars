using Locompro.Data;
using Locompro.Models;
using Locompro.Data.Repositories;

namespace Locompro.Services.Domain
{
    /// <summary>
    /// Service for Category
    /// </summary>
    public class CategoryService : DomainService<Category, string>
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork"></param>
        /// <param name="loggerFactory"></param>
        public CategoryService(IUnitOfWork unitOfWork, ILoggerFactory loggerFactory) : base(unitOfWork, loggerFactory)
        {
        }
    }
}
