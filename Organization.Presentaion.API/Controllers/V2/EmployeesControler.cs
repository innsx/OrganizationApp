using Microsoft.AspNetCore.Mvc;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Company.Models;
using Organization.Domain.Employees;
using Organization.Domain.Employees.Models;

namespace Organization.Presentaion.API.Controllers.V2
{
    //[Route("api/[controller]")]   //setup for Query String or HEADER API Versioning
    [Route("api/v{v:apiVersion}/[controller]")] //setup for URI API Versioning
    [ApiVersion("2.0")]  //specified version
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public EmployeesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// This endpoint gets all the Employees in the system.
        /// </summary>
        /// <respone code="200">Returns paged list of all Employees in the system</respone>
        [ProducesResponseType(typeof(PageList<EmployeeResponseDto>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeQueryParameters employeeQueryParameters)
        {
            var employees = await _unitOfWork.Employees.GetEmployeesByQueryAsync(employeeQueryParameters);
            return Ok(employees);
        }

        /// <summary>
        /// This endpoint gets a particular Employee from the system based on the provided Employee id.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <response code="200">Gets a Employee successfully.</response>
        /// <response code="404">Could not find the Employee.</response>
        /// <returns>Company</returns>
        [HttpGet("{id:length(22)}")]
        public async Task<IActionResult> GetEmployeeById(string id)
        {
            //var employee = await _unitOfWork.Employees.GetByIdAsync(id);
            var employee = await _unitOfWork.Employees.GetByIdAsync(id);

            return Ok(employee);
        }

        /// <summary>
        /// This endpoint adds an Employee in the system.
        /// </summary>
        /// <param name="employeeRequestDto">**CreateEmployeeRequest**</param>
        /// <response code="201">Adds an Employee successfullly</response>
        [HttpPost("employee")]
        public async Task<IActionResult> AddEmployee([FromBody] EmployeeRequestDto employeeRequestDto)
        {
            if (employeeRequestDto == null)
            {
                return BadRequest();
            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            string employeeId = await _unitOfWork.Employees.AddAsnyc(
                new Employee     //creating a new Company object & INITIALIZING Company PROPERTIES
                {
                    Name = employeeRequestDto.Name,
                    Age = employeeRequestDto.Age,
                    Position = employeeRequestDto.Position,
                    Salary = employeeRequestDto.Salary,
                    CompanyId = employeeRequestDto.CompanyId,
                }
            );

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            return Ok(CreatedAtAction(nameof(GetEmployeeById), new { id = employeeId }, employeeRequestDto));
        }


        /// <summary>
        /// This endpoint updates a Employee in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="employeeRequestDto">**EmployeeResponse**</param>
        /// <response code="201">Updates a Employee successfullly</response>
        [HttpPut("{id:length(22)}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] EmployeeRequestDto employeeRequestDto)
        {
            if (id == null)
            {
                return BadRequest();
            }

            if (employeeRequestDto == null)
            {
                return NotFound(employeeRequestDto);
            }

            var employeeToUpdate = await _unitOfWork.Employees.GetByIdAsync(id);

            if (employeeToUpdate != null)
            {
                employeeToUpdate.Name = employeeRequestDto.Name;
                employeeToUpdate.Age = employeeRequestDto.Age;
                employeeToUpdate.Position = employeeRequestDto.Position;
                employeeToUpdate.Salary = employeeRequestDto.Salary;
                employeeToUpdate.CompanyId = employeeRequestDto.CompanyId;
                employeeToUpdate.ModifiedOn = DateTime.Now;
            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            await _unitOfWork.Employees.UpdateAsync(employeeToUpdate!);

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            return Ok(CreatedAtAction(nameof(GetEmployeeById), new { id }, employeeToUpdate));
        }


        /// <summary>
        /// This endpoint SoftDeletes of an Employee in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="isSoftDeleteRecordHasRelatedChildTableColumn">**Boolean**</param>
        /// <response code="201">SoftDeletes an Employee successfullly</response>
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


