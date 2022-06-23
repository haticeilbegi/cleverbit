using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Region;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleverBit.Task1.Application.Abstract
{
    public interface IRegionManager
    {
        Task<Result<List<RegionInputDto>>> GetAll();
        Task<Result<RegionImportDto>> Import(IFormFile file);
        Task<Result<int>> Add(RegionInputDto employee);
    }
}
