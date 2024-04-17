using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Employee;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications.EmployeeSpecs;

namespace Talabat.APIs.Controllers
{
    
    public class EmployeesController : BaseApiController
    {
        private readonly IGenaricRepository<Employee> _employeeRepo;

        public EmployeesController(IGenaricRepository<Employee> employeeRepo) 
        {
            _employeeRepo = employeeRepo;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var spec = new EmployeeWithDepartmentSpecifications();
            var employees = await _employeeRepo.GetAllWithSpecAsync(spec);
            return Ok(employees);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployee(int id)
        {
            var spec = new EmployeeWithDepartmentSpecifications(id);
            var employee = await _employeeRepo.GetWithSpecAsync(spec);
            if (employee is null)
            {
                return NotFound(new { message = "Not found", statsCode = 404 }); 
            }

            return Ok(employee);
        }

    }
}
