using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Employee;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleverBit.Task1.Services.Abstract
{
    public interface IEmployeeService
    {
        Task<List<EmployeeInputDto>> GetAll();
        Task<Result> Import(List<EmployeeInputDto> importList);
        Task<List<EmployeeDto>> GetByRegionId(int regionId);
        Task<int> Add(EmployeeInputDto employee);
    }
}
