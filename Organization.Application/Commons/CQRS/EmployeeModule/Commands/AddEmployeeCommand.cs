using ErrorOr;
using MediatR;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    //public record AddEmployeeCommand(AddEmployeeRequestDto addEmployeeRequestDto) : IRequest<string>;
    public record AddEmployeeCommand(string Name,
        int Age,
        string Position,
        decimal Salary,
        DateTime CreatedOn,
        DateTime ModifiedOn,
        string CompanyId
    ) : IRequest<ErrorOr<Unit>>;
}
