using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeFunctions.Models
{
    public partial class EmployeeDbContext : DbContext
    {
        public EmployeeDbContext() { }

        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options) : base(options) { }

        public virtual DbSet<Employee> Employees { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
        }
    }
}
