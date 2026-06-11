using Microsoft.AspNetCore.Mvc;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Employees;
using Organization.Domain.Employees.Models;

namespace Organization.Presentaion.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeQueryParameters employeeQueryParameters)
        {
            var employees = await _unitOfWork.Employees.GetEmployeesByQueryAsync(employeeQueryParameters);
            return Ok(employees);
        }


        [HttpGet("{id:length(22)}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            return Ok(employee);
        }

        [HttpPost("employee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeRequestDto employeeRequest)
        {
            if (employeeRequest == null)
            {
                return BadRequest();
            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            string employeeId = await _unitOfWork.Employees.AddAsnyc(
                new Employee     //creating a new Company object & INITIALIZING Company PROPERTIES
                {
                    Name = employeeRequest.Name,
                    Age = employeeRequest.Age,
                    Position = employeeRequest.Position,
                    Salary = employeeRequest.Salary,
                    CompanyId = employeeRequest.CompanyId,
                }
            );

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            return Ok(CreatedAtAction(nameof(GetEmployeeById), new {id = employeeId}, employeeRequest));
        }

        [HttpPut("{id:length(22)}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] EmployeeRequestDto employeeRequest)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (employeeRequest == null)
            {
                return NotFound(employeeRequest);
            }

            var employeeToUpdate = await _unitOfWork.Employees.GetByIdAsync(id);

            if (employeeToUpdate != null)
            {
                employeeToUpdate.Name = employeeRequest.Name;
                employeeToUpdate.Age = employeeRequest.Age;
                employeeToUpdate.Position = employeeRequest.Position;
                employeeToUpdate.Salary = employeeRequest.Salary;
                employeeToUpdate.CompanyId = employeeRequest.CompanyId;
            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            await _unitOfWork.Employees.UpdateAsync(employeeToUpdate!);

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            return Ok(CreatedAtAction(nameof(GetEmployeeById), new { id = id }, employeeRequest));
        }

        [HttpDelete("{id:length(22)}")]
        public async Task<IActionResult> DeleteEmployee(string id, [FromBody] bool isSoftDeleteRecordHasRelatedChildTableColumn = false)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var employeeToDelete = await _unitOfWork.Employees.GetByIdAsync(id);

            if (employeeToDelete == null)
            {
                return NotFound(employeeToDelete);
            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            await _unitOfWork.Employees.SoftDeleteAsync(id, isSoftDeleteRecordHasRelatedChildTableColumn);

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            return Ok("Employee successfully softDeleted.");
        }
    }
}

