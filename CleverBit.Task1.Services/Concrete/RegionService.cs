using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Region;
using CleverBit.Task1.Data.Entities;
using CleverBit.Task1.Data.Shared.Abstract;
using CleverBit.Task1.Services.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBit.Task1.Services.Concrete
{
    public class RegionService : IRegionService
    {
        private readonly IEFRepository<Region> _regionRepository;
        public RegionService(IEFRepository<Region> regionRepository)
        {
            _regionRepository = regionRepository;
        }

        public async Task<List<RegionInputDto>> GetAll()
        {
            var data = await _regionRepository.GetAllListAsync();
            var result = data.Select(x => new RegionInputDto
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId
            }).ToList();

            return result;
        }

        public async Task<Result> Import(List<RegionInputDto> importList)
        {
            // parents first so we wouldnt get error because of the foreign key
            var partition1 = importList.Where(x => x.ParentId == null).Select(x => new Region
            {
                Name = x.Name
            }).ToList();
            await _regionRepository.InsertAsync(partition1);

            // and now the rest of them
            var partition2 = importList.Where(x => x.ParentId != null).Select(x => new Region
            {
                Name = x.Name,
                ParentId = x.ParentId
            }).ToList();
            await _regionRepository.InsertAsync(partition2);

            var result = importList.Select(x => new RegionInputDto
            {
                Id = x.Id,
                Name = x.Name,
                ParentId = x.ParentId
            }).ToList();

            return new Result("Success", true);
        }

        public async Task<Dictionary<int, string>> GetSubRegions(int parentId)
        {
            var data = _regionRepository.GetAll().Where(x => x.ParentId == parentId)
                .Select(x => new { x.Id, x.Name })
                .ToDictionary(x => x.Id, y => y.Name);
            return data;
        }

        public async Task<int> Add(RegionInputDto region)
        {
            var entity = new Region
            {
                Id = region.Id,
                Name = region.Name,
                ParentId = region.ParentId
            };

            var id = await _regionRepository.InsertAndGetIdAsync(entity);
            return id;
        }
    }
}
