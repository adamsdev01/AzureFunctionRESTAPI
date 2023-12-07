using System.Collections.Generic;
using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using EmployeeFunctions.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System.Linq;

namespace EmployeeFunctions
{
    public class GetEmployeeByEmpCodeFunction
    {
        private readonly ILogger<GetEmployeeByEmpCodeFunction> _logger;

        public GetEmployeeByEmpCodeFunction(ILogger<GetEmployeeByEmpCodeFunction> log)
        {
            _logger = log;
        }

        [FunctionName("GetEmployeeByEmpCodeFunction")]
        [OpenApiOperation(operationId: "Run")]
        [OpenApiParameter(name: "empCode", In = ParameterLocation.Query, Required = true, Type = typeof(string), Description = "The **EmpCode** parameter")]
        [OpenApiResponseWithBody(statusCode: HttpStatusCode.OK, contentType: "application/json", bodyType: typeof(Employee), Description = "The OK response")]
        public async Task<IActionResult> Run(
           [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "GetEmployeeByEmpCode/{empCode}")] HttpRequest req, string? empCode)
        {
            try
            {
                string defaultConnection = Environment.GetEnvironmentVariable("DbConnection");

                _logger.LogInformation(defaultConnection);

                var options = new DbContextOptionsBuilder<EmployeeDbContext>();
                options.UseSqlServer(defaultConnection);

                var _dbContext = new EmployeeDbContext(options.Options);

                var employee = await _dbContext.Employees.Where(e => e.EmployeeCode == empCode).FirstOrDefaultAsync();

                if (employee != null)
                {
                    var result = new
                    {
                        id = employee.Id,
                        employeeCode = employee.EmployeeCode,
                        firstName = employee.FirstName,
                        lastName = employee.LastName,
                        dob = employee.DOB,
                        address1 = employee.Address1,
                        address2 = employee.Address2,
                        city = employee.City,
                        state = employee.State,
                        zipCode = employee.ZipCode,
                        country = employee.Country,
                        phoneNumber = employee.PhoneNumber,
                        emailAddress = employee.EmailAddress,
                        startDate = employee.StartDate
                    };

                    return new OkObjectResult(result);
                }

                return new NotFoundResult();
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());

                return new StatusCodeResult((int)HttpStatusCode.InternalServerError);
            }
        }

    }
}

