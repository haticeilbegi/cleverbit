using CleverBit.Task1.Application.Abstract;
using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Employee;
using CleverBit.Task1.Services.Abstract;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBit.Task1.Application.Concrete
{
    public class EmployeeManager : IEmployeeManager
    {
        private readonly IEmployeeService _employeeService;
        public EmployeeManager(IEmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        public async Task<Result<List<EmployeeInputDto>>> GetAll()
        {
            var result = await _employeeService.GetAll();
            return new Result<List<EmployeeInputDto>>("Success", true, result);
        }

        public async Task<Result<EmployeeImportDto>> Import(IFormFile file)
        {
            var importList = new List<EmployeeInputDto>();

            using (var fileStream = file.OpenReadStream())
            using (var reader = new StreamReader(fileStream))
            {
                string row;
                while ((row = reader.ReadLine()) != null)
                {
                    if (!string.IsNullOrEmpty(row))
                    {
                        var line = row.Split(',');
                        var lineData = new EmployeeInputDto
                        {
                            RegionId = int.Parse(line[0]),
                            FirstName = line[1],
                            LastName = line[2]
                        };
                        importList.Add(lineData);
                    }
                }
            }

            if (importList == null && !importList.Any())
                return new Result<EmployeeImportDto>("NoRecord", true, null);

            var result = await _employeeService.Import(importList);
            if (!result.IsSuccess)
                return new Result<EmployeeImportDto>("Error", false, null);

            return new Result<EmployeeImportDto>("Success", true, new EmployeeImportDto { Employees = importList });
        }

        public async Task<Result<List<EmployeeDto>>> GetByRegionId(int regionId)
        {
            var result = await _employeeService.GetByRegionId(regionId);
            return new Result<List<EmployeeDto>>("Success", true, result);
        }

        public async Task<Result<int>> Add(EmployeeInputDto employee)
        {
            var result = await _employeeService.Add(employee);
            return new Result<int>("Success", true, result);
        }
    }
}
