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
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace EmployeeFunctions
{
    public  class EmployeeApi
    {
        private readonly EmployeeDbContext _dbContext;

        public EmployeeApi(EmployeeDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        static List<Employees> employeesList = new();


        [FunctionName("GetEmployees")]
        public async Task<IActionResult> GetEmployees(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employee")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("Getting all employees.");

            string requestData = await new StreamReader(req.Body).ReadToEndAsync();

            // deserialize into Employee Item
            var data = JsonConvert.DeserializeObject<CreateEmployeeItem>(requestData);

            List<Employees> allEmployees = await _dbContext.Employees.ToListAsync();

            return new OkObjectResult(allEmployees);
        }

        [FunctionName("GetEmployeeByEmployeeCode")]
        public static async Task<IActionResult> GetEmployeeByEmployeeCode(
          [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "employee/{employeeCode}")] 
            HttpRequest req, ILogger log, string employeeCode)
        {
            log.LogInformation($"Get employee record with employee code: {employeeCode}");

            var employeeResult = employeesList.FirstOrDefault(e => e.EmployeeCode == employeeCode);
            if (employeeResult == null) 
            {
                return new NotFoundResult();
            }

            return new OkObjectResult(employeeResult);
        }


        [FunctionName("CreateEmployee")]
        public static async Task<IActionResult> CreateEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employee")] HttpRequest req,
            ILogger log, EmployeeDbContext dbContext)
        {
            log.LogInformation("Creating An Employee.");

            string requestData = await new StreamReader(req.Body).ReadToEndAsync();

            // deserialize into Employee Item
            var data = JsonConvert.DeserializeObject<CreateEmployeeItem>(requestData);

            //// Retrieve connection string from configuration
            //string connectionString = Configuration.GetConnectionString("SqlConnectionString");

            //// Create a DbContext with the connection string
            //var options = new DbContextOptionsBuilder<EmployeeDbContext>()
            //    .UseSqlServer(connectionString)
            //    .Options;

            //using (var dbContext = new EmployeeDbContext(options))
            //{

            //    // Add a new employee entry
            //    var newEmployee = new Employee
            //    {
            //        DOB = data.DOB,
            //        FirstName = data.FirstName,
            //        LastName = data.LastName,
            //    };

            //    dbContext.EmployeeSet.Add(newEmployee);
            //    await dbContext.SaveChangesAsync();
            //}

            var newEmployee = new Employees();

            dbContext.Employees.Add(newEmployee);
            await dbContext.SaveChangesAsync();

            return new OkObjectResult(data);
        }

        [FunctionName("PutEmployee")]
        public static async Task<IActionResult> PutEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "employee/{Id}")] 
            HttpRequest req, ILogger log, int Id)
        {
            log.LogInformation($"Updating employee record with Id: {Id}");

            var employeeResult = employeesList.FirstOrDefault(e => e.Id == Id);
            if (employeeResult == null)
            {
                return new NotFoundResult();
            }

            string requestData = await new StreamReader(req.Body).ReadToEndAsync();

            // deserialize into Upate Employee Item
            var data = JsonConvert.DeserializeObject<UpdateEmployeeItem>(requestData);

            employeeResult.FirstName = data.FirstName;
            employeeResult.LastName = data.LastName;

            return new OkObjectResult(employeeResult);
           
        }

        [FunctionName("DeleteEmployee")]
        public static async Task<IActionResult> DeleteEmployee(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "employee/{Id}")]
            HttpRequest req, ILogger log, int Id)
        {
            log.LogInformation($"Deleting employee record with employee Id: {Id}");

            var employeeResult = employeesList.FirstOrDefault(e => e.Id == Id);
            if (employeeResult == null)
            {
                return new NotFoundResult();
            }

            employeesList.Remove(employeeResult);
            return new OkResult();

        }
    }

}
