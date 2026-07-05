using ErrorOr;
using MediatR;
using Organization.Application.Commons.DTOs;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    //public record AddEmployeeCommand(AddEmployeeRequestDto employeeRequestDto) : IRequest<string>;
    public record AddEmployeeCommand(AddEmployeeRequestDto employeeRequestDto) : IRequest<ErrorOr<Unit>>;
}
