using Locompro.Models;

namespace Locompro.Services.Domain;

public interface ICantonService
{
    Task<Canton> Get(string country, string province, string canton);
}