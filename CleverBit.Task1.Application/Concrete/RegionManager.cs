using CleverBit.Task1.Application.Abstract;
using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Region;
using CleverBit.Task1.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBit.Task1.Application.Concrete
{
    public class RegionManager : IRegionManager
    {
        private readonly IRegionService _regionService;
        public RegionManager(IRegionService regionService)
        {
            _regionService = regionService;
        }

        public async Task<Result<List<RegionInputDto>>> GetAll()
        {
            var result = await _regionService.GetAll();
            return new Result<List<RegionInputDto>>("Success", true, result);
        }

        public async Task<Result<RegionImportDto>> Import(IFormFile file)
        {
            var importList = new List<RegionInputDto>();

            using (var fileStream = file.OpenReadStream())
            using (var reader = new StreamReader(fileStream))
            {
                string row;
                while ((row = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        var line = row.Split(',');
                        var lineData = new RegionInputDto
                        {
                            Name = line[0],
                            Id = int.Parse(line[1])
                        };

                        if (line.Length == 3 && !string.IsNullOrEmpty(line[2]))
                            lineData.ParentId = int.Parse(line[2]);

                        importList.Add(lineData);
                    }
                }
            }

            if (importList == null && !importList.Any())
                return new Result<RegionImportDto>("NoRecord", true, null);

            var result = await _regionService.Import(importList);
            if (!result.IsSuccess)
                return new Result<RegionImportDto>("Error", false, null);

            return new Result<RegionImportDto>("Success", true, new RegionImportDto { Regions = importList });
        }

        public async Task<Result<int>> Add(RegionInputDto employee)
        {
            var result = await _regionService.Add(employee);
            return new Result<int>("Success", true, result);
        }
    }
}
