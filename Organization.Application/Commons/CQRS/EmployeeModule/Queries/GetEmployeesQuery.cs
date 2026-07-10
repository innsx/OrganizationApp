using ErrorOr;
using MediatR;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Employees;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Queries
{
    public record GetEmployeesQuery(EmployeeQueryParameters employeeQueryParameters) : IRequest<PageList<EmployeeResponseDto>>;
}
