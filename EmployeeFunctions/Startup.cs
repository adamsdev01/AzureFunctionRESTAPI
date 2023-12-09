﻿using EmployeeFunctions.Models;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[assembly: FunctionsStartup(typeof(EmployeeFunctions.Startup))]

namespace EmployeeFunctions
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            string connectionString = Environment.GetEnvironmentVariable("DbConnection");

            builder.Services.AddDbContext<EmployeeDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            
            });
        }
    }
}
