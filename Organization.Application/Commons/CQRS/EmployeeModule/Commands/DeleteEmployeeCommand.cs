using ErrorOr;
using MediatR;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    public record DeleteEmployeeCommand(string Id, bool isDeleteHasAssociations) : IRequest<ErrorOr<Unit>>;
}
