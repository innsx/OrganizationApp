using MediatR;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.Interfaces.Persistance;
using ErrorOr; 
using Organization.Domain.Commons.Errors;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Queries
{
    public class GetEmployeeByIdQueryHandler : IRequestHandler<GetEmployeeByIdQuery, ErrorOr<EmployeeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeeByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<EmployeeResponseDto>> Handle(GetEmployeeByIdQuery request, CancellationToken cancellationToken)
        {
            var employee = await _unitOfWork.Employees.GetByIdAsync(request.id);

            if (employee == null)
            {
                //1st approach: commented this line
                //return NotFound(employee);

                //2nd approach: Throw customized exception
                //throw new EmployeeNotFoundException($"The system does not have any Employee with id = {request.id}");

                //3rd approach: using ErrorOr                            
                var errorMessage = Errors.Employee.EmployeeDoesNotExist($"The system does not have any Employee with Id: ${request.id}");

                return errorMessage;
            }

            return new EmployeeResponseDto(               
                employee.Name,
                employee.Age,
                employee.Position,
                employee.Salary,
                employee.CreatedOn,
                employee.ModifiedOn,
                employee.CompanyId
             );

        }

    }
}




//if (hasAssociatedObject is false)
//{
//    //var employee = await _unitOfWork.Employees.GetByIdAsync(id);
//    var employee = await _unitOfWork.Employees.GetByIdAsync(id);

//    if (employee == null)
//    {
//        //commented this line
//        //return NotFound(employee);

//        //add this line
//        throw new NotFoundException($"The system does not have any Employee with id = {id}");
//    }

//    return Ok(employee);
//}
//else
//{
//    //var employee = await employeeRepository.QueryOneToManyParentChildRelationshipAsync(id);

//    var employee = await _unitOfWork.Employees.QueryOneToManyParentChildRelationshipAsync(id);

//    if (employee is null)
//    {
//        //add this line
//        throw new NotFoundException($"The system does not have any Company with id = {id}");
//    }

//    return Ok(employee);
//}