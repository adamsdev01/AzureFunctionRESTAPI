using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using EmployeeFunctions.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Net.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using System.Net;

namespace EmployeeFunctions
{
    public class GetEmployeesFunction
    {
        private readonly ILogger _logger;
        private readonly HttpClient _httpClient;
        public GetEmployeesFunction(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<GetEmployeesFunction>();
        }

        [FunctionName("GetEmployees")]
        [OpenApiOperation(operationId: "Run")]
        [OpenApiSecurity("function_key", SecuritySchemeType.ApiKey, Name = "code", In = OpenApiSecurityLocationType.Query)]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "text/plain", bodyType: typeof(string), Description = "The OK response")]
        public async Task<List<Employee>> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "GetEmployees")] HttpRequest req)
        {
            var employeesList = new List<Employee>();
            try
            {
                string defaultConnection = Environment.GetEnvironmentVariable("DbConnection");

                _logger.LogInformation(defaultConnection);
                var options = new DbContextOptionsBuilder<EmployeeDbContext>();
                options.UseSqlServer(defaultConnection);

                var _dbContext = new EmployeeDbContext(options.Options);

                employeesList = await _dbContext.Employees.ToListAsync();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
            }

            return employeesList;

        }
    }
    
}
