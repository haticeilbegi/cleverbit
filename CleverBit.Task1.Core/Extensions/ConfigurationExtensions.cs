using CleverBit.Task1.Application.Abstract;
using CleverBit.Task1.Application.Concrete;
using CleverBit.Task1.Common.Helpers;
using CleverBit.Task1.Core.Middlewares;
using CleverBit.Task1.Data;
using CleverBit.Task1.Data.Shared.Abstract;
using CleverBit.Task1.Data.Shared.Concrete;
using CleverBit.Task1.Services.Abstract;
using CleverBit.Task1.Services.Concrete;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleverBit.Task1.Core.Extensions
{
    public static class ConfigurationExtensions
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }

        public static IServiceCollection ConfigureDatabase(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<DbContext, CleverBitDbContext>(options =>
            {
                var connectionstring = configuration.GetValue<string>("DbConnection");
                options.UseSqlServer(connectionstring);
            });

            return serviceCollection;
        }

        public static IServiceCollection ConfigureDependencies(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped(typeof(IEFRepository<>), typeof(EFRepository<>));

            serviceCollection.AddScoped<IHttpRequestHelper, HttpRequestHelper>();

            serviceCollection.AddScoped<IRegionService, RegionService>();
            serviceCollection.AddScoped<IRegionManager, RegionManager>();

            serviceCollection.AddScoped<IEmployeeService, EmployeeService>();
            serviceCollection.AddScoped<IEmployeeManager, EmployeeManager>();

            return serviceCollection;
        }
    }
}
