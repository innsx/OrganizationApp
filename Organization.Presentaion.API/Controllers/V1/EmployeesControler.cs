using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Commons.CQRS.EmployeeModule.Commands;
using Organization.Application.Commons.CQRS.EmployeeModule.Queries;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Company.Models;
using Organization.Domain.Employees;
using Organization.Domain.Employees.Models;

namespace Organization.Presentaion.API.Controllers.V1
{
    //[Route("api/[controller]")]   //setup for Query String or HEADER API Versioning
    [Route("api/v{v:apiVersion}/[controller]")] //setup for URI API Versioning
    [ApiVersion("1.0")]  //specified version
    [ApiController]
    [Produces("application/json")]
    public sealed class EmployeesController : BaseAPIController
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ISender _sender;
        private readonly IMapper _mapper;

        public EmployeesController(IUnitOfWork unitOfWork, ISender sender, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _sender = sender;
            _mapper = mapper;
        }


        /// <summary>
        /// This endpoint gets all the Employees in the system.
        /// </summary>
        /// <respone code="200">Returns paged list of all Employees in the system</respone>
        [ProducesResponseType(typeof(PageList<EmployeeResponseDto>), 200)]
        [HttpGet]
        public async Task<IActionResult> GetEmployees([FromQuery] EmployeeQueryParameters employeeQueryParameters)
        {
            var employees = await _sender.Send(new GetEmployeesQuery(employeeQueryParameters));
            
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
        public async Task<IActionResult> GetEmployeeById(string id) //bool hasAssociatedObject = false
        {
            var employee = await _sender.Send(new GetEmployeeByIdQuery(id));
            return Ok(employee);
        }

        /// <summary>
        /// This endpoint adds an Employee in the system.
        /// </summary>
        /// <param name="addEmployeeRequestDto">**CreateEmployeeRequest**</param>
        /// <response code="201">Adds an Employee successfullly</response>
        [HttpPost("employee")]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeRequestDto addEmployeeRequestDto)
        {
            var employeeId = await _sender.Send(new AddEmployeeCommand(addEmployeeRequestDto));

            return Ok(CreatedAtAction(nameof(GetEmployeeById), new {id = employeeId}, addEmployeeRequestDto));
        }


        /// <summary>
        /// This endpoint updates a Employee in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="updateEmployeeRequestDto">**EmployeeResponse**</param>
        /// <response code="201">Updates a Employee successfullly</response>
        [HttpPut("{id:length(22)}")]
        public async Task<IActionResult> UpdateEmployee(string id, [FromBody] UpdateEmployeeRequestDto updateEmployeeRequestDto)
        {
            //manually mapping the EmployeeRequestDto to UpdateEmployeeCommand
            //await _sender.Send(new UpdateEmployeeCommand(
            //    id, 
            //    employeeRequestDto.Name, 
            //    employeeRequestDto.Age, 
            //    employeeRequestDto.Position, 
            //    employeeRequestDto.Salary, 
            //    employeeRequestDto.CreatedOn,
            //    employeeRequestDto.ModifiedOn, 
            //    employeeRequestDto.CompanyId
            //    ));

            //using MapsterMapper to map the EmployeeRequestDto to UpdateEmployeeCommand
            var mappedEmployee = _mapper.Map<UpdateEmployeeCommand>((id, updateEmployeeRequestDto));

            await _sender.Send(mappedEmployee);


            return Ok(CreatedAtAction(nameof(GetEmployeeById), new { id }, mappedEmployee));

        }


        /// <summary>
        /// This endpoint SoftDeletes of an Employee in the system.
        /// </summary>
        /// <param name="id">**string**</param>
        /// <param name="isRecordHasAssociation">**Boolean**</param>
        /// <response code="201">SoftDeletes an Employee successfullly</response>
        [HttpDelete("{id:length(22)}")]
        public async Task<IActionResult> DeleteEmployee(string id, bool isRecordHasAssociation = false)
        {
            if (id == null)
            {
                return BadRequest();
            }

            await _sender.Send(new DeleteEmployeeCommand(id, isRecordHasAssociation));

            return Ok("Employee successfully softDeleted.");
        }
    }
}

