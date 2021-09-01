using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace API.Controllers.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _environment;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IHostEnvironment environment)
        {
            _environment = environment;
            _logger = logger;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
               await _next(context); 
            }
            catch (Exception ex)
            {
               _logger.LogError(ex, ex.Message);
               context.Response.ContentType = "application/json";
               context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

               var response = _environment.IsDevelopment() ? new APIException((int)HttpStatusCode.InternalServerError,ex.Message,ex.StackTrace.ToString())
               : new APIException((int)HttpStatusCode.InternalServerError);

               var options = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
               var json = JsonSerializer.Serialize(response , options);

               await context.Response.WriteAsync(json); 
            }
        }
    }
}