using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Employee;
using CleverBit.Task1.Common.Models.Dto.Region;
using CleverBit.Task1.Data.Entities;
using CleverBit.Task1.Data.Shared.Abstract;
using CleverBit.Task1.Services.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBit.Task1.Services.Concrete
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEFRepository<Employee> _employeeRepository;
        private readonly IRegionService _regionService;
        public EmployeeService(IEFRepository<Employee> employeeRepository, IRegionService regionService)
        {
            _employeeRepository = employeeRepository;
            _regionService = regionService;
        }

        public async Task<List<EmployeeInputDto>> GetAll()
        {
            var data = await _employeeRepository.GetAllListAsync();
            var result = data.Select(x => new EmployeeInputDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                RegionId = x.RegionId
            }).ToList();

            return result;
        }

        public async Task<Result> Import(List<EmployeeInputDto> importList)
        {
            var partition2 = importList.Select(x => new Employee
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                RegionId = x.RegionId
            }).ToList();
            await _employeeRepository.InsertAsync(partition2);

            var result = importList.Select(x => new EmployeeInputDto
            {
                Id = x.Id,
                FirstName = x.FirstName,
                LastName = x.LastName,
                RegionId = x.RegionId
            }).ToList();

            return new Result("Success", true);
        }

        public async Task<List<EmployeeDto>> GetByRegionId(int regionId)
        {
            var subRegions = await _regionService.GetSubRegions(regionId);
            var result = _employeeRepository.GetAllIncluding(x => x.Region)
                .Where(x => x.RegionId == regionId || subRegions.Keys.Contains(x.RegionId))
                .Select(x => new
                {
                    EmployeeName = $"{x.FirstName} {x.LastName}",
                    Region = x.Region
                })
                .AsEnumerable()
                .GroupBy(x => x.EmployeeName)
                .Select(x => new EmployeeDto
                {
                    EmployeeName = x.Key,
                    Regions = x.Select(a => new RegionDto { Id = a.Region.Id, Name = a.Region.Name }).ToList()
                })
                .ToList();

            return result;
        }

        public async Task<int> Add(EmployeeInputDto employee)
        {
            var entity = new Employee
            {
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                RegionId = employee.RegionId
            };

            var id = await _employeeRepository.InsertAndGetIdAsync(entity);
            return id;
        }
    }
}
