using CleverBit.Task1.Common.Models.Dto.Region;
using System;
using System.Collections.Generic;
using System.Text;

namespace CleverBit.Task1.Common.Models.Dto.Employee
{
    public class EmployeeDto
    {
        public int Id { get; set; }
        public string EmployeeName { get; set; }
        public List<RegionDto> Regions { get; set; }
    }
}
