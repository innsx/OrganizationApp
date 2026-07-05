using ErrorOr;
using MediatR;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    public record UpdateEmployeeCommand(
        string Id,
        string Name, 
        int Age, 
        string Position, 
        decimal Salary,  
        DateTime CreatedOn,
        DateTime ModifiedOn,
        string CompanyId) : IRequest<ErrorOr<Unit>>;
}
