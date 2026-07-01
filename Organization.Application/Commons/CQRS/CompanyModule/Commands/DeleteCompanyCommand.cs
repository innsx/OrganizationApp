using MediatR;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    public record DeleteCompanyCommand(string Id, bool isDeleteHasAssociations) : IRequest;
}
