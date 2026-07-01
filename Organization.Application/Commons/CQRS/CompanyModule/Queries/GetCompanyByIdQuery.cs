using MediatR;
using Organization.Application.Commons.DTOs;

namespace Organization.Application.Commons.CQRS.CompanyModule.Queries
{
    public record GetCompanyByIdQuery(string Id, bool hasAssociatedObject) : IRequest<CompanyResponseDto>;
}
