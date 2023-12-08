using System.Text.Json.Serialization;

namespace EmployeeFunctions.Blazor.Models
{
    internal class Employee
    {
        public int Id { get; set; }

        public string EmployeeCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime DOB { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }
        public string? Country { get; set; }

        public string? PhoneNumber { get; set; }

        public string? EmailAddress { get; set; }

        public DateTime? StartDate { get; set; }
    }

    internal class CreateEmployeeItem
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("employeeCode")]
        public string EmployeeCode { get; set; }

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }

        [JsonPropertyName("lastName")]
        public string LastName { get; set; }

        [JsonPropertyName("dob")]
        public DateTime DOB { get; set; }

        public string? Address1 { get; set; }

        public string? Address2 { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }
        public string? Country { get; set; }

        public string? PhoneNumber { get; set; }

        public string? EmailAddress { get; set; }

        public DateTime? StartDate { get; set; }
    }

    internal class UpdateEmployeeItem
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }
    }
}
