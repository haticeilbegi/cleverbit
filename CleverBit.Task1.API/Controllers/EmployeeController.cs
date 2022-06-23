using CleverBit.Task1.Application.Abstract;
using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Employee;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBit.Task1.API.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IEmployeeManager _employeeManager;
        public EmployeeController(IEmployeeManager employeeManager)
        {
            _employeeManager = employeeManager;
        }

        [HttpGet]
        [Route("api/employees")]
        public async Task<List<EmployeeInputDto>> Get()
        {
            var result = await _employeeManager.GetAll();
            return result.Data;
        }

        [HttpGet]
        [Route("api/region/{id}/employees")]
        public async Task<Result<List<EmployeeDto>>> Get([FromRoute(Name = "id")] int regionId)
        {
            var result = await _employeeManager.GetByRegionId(regionId);
            return result;
        }

        [HttpPost]
        [Route("api/employee")]
        public async Task<Result<int>> Post([FromBody] EmployeeInputDto employeeDto)
        {
            var result = await _employeeManager.Add(employeeDto);
            return result;
        }

        [HttpPost]
        [Route("api/employee/import")]
        public async Task<Result> Import([FromBody] IFormFile file)
        {
            var result = await _employeeManager.Import(file);
            return result;
        }
    }
}
