using MediatR;

namespace Organization.Application.Commons.CQRS.CompanyModule.Queries.GetCompanyCount
{
    public record GetCompanyCountQuery : IRequest<int>
    {
    }
}
