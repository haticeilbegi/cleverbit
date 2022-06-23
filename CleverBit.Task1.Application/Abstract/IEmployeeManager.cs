using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Employee;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleverBit.Task1.Application.Abstract
{
    public interface IEmployeeManager
    {
        Task<Result<List<EmployeeInputDto>>> GetAll();
        Task<Result<EmployeeImportDto>> Import(IFormFile file);
        Task<Result<List<EmployeeDto>>> GetByRegionId(int regionId);
        Task<Result<int>> Add(EmployeeInputDto employee);
    }
}
