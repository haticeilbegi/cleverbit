using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Region;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleverBit.Task1.Services.Abstract
{
    public interface IRegionService
    {
        Task<List<RegionInputDto>> GetAll();
        Task<Result> Import(List<RegionInputDto> importList);
        Task<Dictionary<int, string>> GetSubRegions(int parentId);
        Task<int> Add(RegionInputDto region);
    }
}
