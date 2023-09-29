using Locompro.Repositories;
using Locompro.Models;

namespace Locompro.Services
{
    public class AdministrativeUnitService : AbstractService<Province, string, ProvinceRepository>
    {

        CantonRepository cantonRepository;

        public AdministrativeUnitService(UnitOfWork unitOfWork,
                ProvinceRepository repository,
                CantonRepository cantonRepository) : base(unitOfWork, repository)
        {
            this.cantonRepository = cantonRepository;
        }

        public async Task<IEnumerable<Province>> GetAllProvinces()
        {
            return await this.repository.GetAllProvinces();
        }

        public async Task<IEnumerable<Canton>> GetCantons(String provinceName)
        {
            return await this.cantonRepository.GetCantons(provinceName);
        }
    }
}
