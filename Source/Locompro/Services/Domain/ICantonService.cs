using Locompro.Models.Entities;

namespace Locompro.Services.Domain;

public interface ICantonService : IDomainService<Canton, string>
{
    Task<Canton> Get(string country, string province, string canton);
}