using CleverBit.Task1.Common.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CleverBit.Task1.Core.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        public ExceptionHandlerMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {
            _next = next;
            _loggerFactory = loggerFactory;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next.Invoke(httpContext);
            }
            catch (Exception ex)
           {
                var categoryName = httpContext.Request.Path.Value?.Replace("/", "");
                _loggerFactory.CreateLogger(categoryName).LogError(ex, "An error occured");

                var response = JsonConvert.SerializeObject(new Result<string>("An error occured", false, ex.Message));
                var responseBytes = Encoding.UTF8.GetBytes(response);
                await httpContext.Response.Body.WriteAsync(responseBytes, 0, responseBytes.Length);
            }
        }
    }
}
