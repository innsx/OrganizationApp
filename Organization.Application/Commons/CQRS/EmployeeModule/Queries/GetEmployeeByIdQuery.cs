using ErrorOr;
using MediatR;
using Organization.Application.Commons.DTOs;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Queries
{
    public record GetEmployeeByIdQuery(string id) : IRequest<ErrorOr<EmployeeResponseDto>>;
}
