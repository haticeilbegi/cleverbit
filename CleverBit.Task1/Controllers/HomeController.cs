using CleverBit.Task1.Common.Enums;
using CleverBit.Task1.Common.Helpers;
using CleverBit.Task1.Common.Models;
using CleverBit.Task1.Common.Models.Dto.Employee;
using CleverBit.Task1.Common.Models.Dto.Region;
using CleverBit.Task1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace CleverBit.Task1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpRequestHelper _httpRequestHelper;
        private string BackEndAPIUrl;

        public HomeController(ILogger<HomeController> logger, IHttpRequestHelper httpRequestHelper, IConfiguration configuration)
        {
            BackEndAPIUrl = configuration.GetValue<string>("BackEndAPIUrl");

            _logger = logger;
            _httpRequestHelper = httpRequestHelper;
        }

        public IActionResult Index()
        {
            return View();
        }

        //[HttpPost]
        //public async Task<Result> Index([FromBody] IFormFile regions, [FromBody] IFormFile employees)
        //{
        //    var regionImportResult = await _httpRequestHelper
        //        .Configure(new IntegrationModel { AuthenticationType = AuthenticationTypes.None, Url = BackEndAPIUrl })
        //        .Post <, String> ("region", ContentTypes.XML, );

        //    return null;
        //}

        public async Task<IActionResult> Regions()
        {
            var result =  await _httpRequestHelper
                .Configure(new IntegrationModel { AuthenticationType = AuthenticationTypes.None, Url = BackEndAPIUrl })
                .Get<List<RegionInputDto>>("/region");

            return View(result.Data);
        }

        public async Task<IActionResult> Employees()
        {
            var result = await _httpRequestHelper
                   .Configure(new IntegrationModel { AuthenticationType = AuthenticationTypes.None, Url = BackEndAPIUrl })
                   .Get<List<EmployeeInputDto>>("/employees");

            return View(result.Data);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
