using Locompro.Models.Entities;

namespace Locompro.Data.Repositories;

public interface ICantonRepository : ICrudRepository<Canton, string>
{
    Task<Canton> GetByIdAsync(string country, string province, string canton);
}