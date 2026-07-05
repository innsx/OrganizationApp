using ErrorOr;
using MediatR;
using Organization.Application.Commons.DTOs;

namespace Organization.Application.Commons.CQRS.CompanyModule.Queries
{
    //instead of throwing an exception, we will returning Errors of typed ErrorOr
    //of the Errors partial class in the Domain layer.
    //This is a better approach than throwing an exception
    //because it allows us to return a more specific error message to the client,
    //and it also allows us to handle the error in a more structured way.
    public record GetCompanyByIdQuery(string Id, bool hasAssociatedObject) : IRequest<ErrorOr<CompanyResponseDto>>;
}
