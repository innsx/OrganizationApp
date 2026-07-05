using ErrorOr;
using MediatR;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    //MediatR.Unit Represent a VOID type, since VOID is not a VALID return type in C#.
    // we use "Unit" to represent a VOID return type in MediatR.
    public record UpdateCompanyCommand(string Id, string Name, string Address, string Country) : IRequest<ErrorOr<Unit>>;
   
}
