using MediatR;
using Organization.Application.Commons.DTOs;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    public record AddEmployeeCommand(EmployeeRequestDto employeeRequestDto) : IRequest<string>;
}
