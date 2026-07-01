using MediatR;
using Organization.Application.Commons.DTOs;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    //RECORDS are immutable -- ONCE CREATED, RECORD CANNOT BE MODIFIED/UPDATED
    public record AddCompanyCommand(string Name, string Address, string Country) : IRequest<string>;
}
