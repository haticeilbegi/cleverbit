using CleverBit.Task1.Application.Abstract;
using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Region;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBit.Task1.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegionController : ControllerBase
    {
        private readonly IRegionManager _regionManager;
        public RegionController(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        [HttpGet]
        public async Task<List<RegionInputDto>> Get()
        {
            var result = await _regionManager.GetAll();
            return result.Data;
        }

        [HttpPost]
        public async Task<Result<int>> Post([FromBody] RegionInputDto regionDto)
        {
            var result = await _regionManager.Add(regionDto);
            return result;
        }

        [HttpPost]
        [Route("import")]
        public async Task<Result> Import([FromBody] IFormFile file)
        {
            var result = await _regionManager.Import(file);
            return result;
        }
    }
}
