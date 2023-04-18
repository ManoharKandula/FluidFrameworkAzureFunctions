using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using FluidFrameworkServerlessDemo;

namespace GetEmpDetails
{
    public static class GetEmpDetails
    {
        [FunctionName("GetEmpDetails")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log,
            [Sql(commandText: "select * from dbo.Employees",
                commandType: System.Data.CommandType.Text,
                connectionStringSetting: "SqlConnectionString")]
                IEnumerable<Employee> employees)
        {
            try
            {
                return new OkObjectResult(employees);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
    public static class PostEmpDetails
    {
        [FunctionName("PostEmpDetails")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)] HttpRequest req,
            ILogger log,
            [Sql(commandText: "dbo.Employees",
                connectionStringSetting: "SqlConnectionString")]
                IAsyncCollector<Employee> employees)
        {
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                Employee employee = JsonConvert.DeserializeObject<Employee>(requestBody);
                await employees.AddAsync(employee);
                await employees.FlushAsync();
                List<Employee> empList = new List<Employee> { employee };
                return new OkObjectResult(empList);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
    public static class DelEmployee
    {
        [FunctionName("DelEmployee")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = null)] HttpRequest req,
            ILogger log,
            [Sql(commandText: "delete from dbo.Employees where EmployeeId = @Id;" +
                              "Select * from dbo.Employees",
                commandType: System.Data.CommandType.Text,
                parameters: "@Id={Query.EmployeeId}",
                connectionStringSetting: "SqlConnectionString")]
                IEnumerable<Employee> employees)
        {
            try
            {
                return new OkObjectResult(employees);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
    public static class GetDepartments
    {
        [FunctionName("GetDepartments")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log,
            [Sql(commandText: "select * from dbo.Department",
                commandType: System.Data.CommandType.Text,
                connectionStringSetting: "SqlConnectionString")]
                IEnumerable<Department> departments)
        {
            try
            {
                return new OkObjectResult(departments);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
