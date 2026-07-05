using MediatR;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Employees.Models;
using ErrorOr;
using Organization.Domain.Commons.Errors;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    public sealed class AddEmployeeCommandHandler : IRequestHandler<AddEmployeeCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(AddEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeNameIsExisted = await _unitOfWork.Employees.IsExistingAsync(request.employeeRequestDto.Name);

            if (employeeNameIsExisted is true)
            {
                //add this line and use DuplicateCompanyException with pass-in specified Company Name if Name is NOT UNIQUE
                //throw new DuplicateNameException($"Employee with Name {request.employeeRequestDto.Name} is ALREADY EXISTED.");

                return Errors.Employee.DuplicateEmployee(request.employeeRequestDto.Name);
            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            var employeeId = await _unitOfWork.Employees.AddAsnyc(
                new Employee
                {
                    Name = request.employeeRequestDto.Name,
                    Age = request.employeeRequestDto.Age,
                    Position = request.employeeRequestDto.Position,
                    Salary = request.employeeRequestDto.Salary,
                    CreatedOn = DateTime.Now,
                    CompanyId = request.employeeRequestDto.CompanyId
                }
            );

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            //return employeeId;
            return Unit.Value;

        }
    }
}
