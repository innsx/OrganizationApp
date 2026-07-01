using MediatR;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Commons.Utilities;
using Organization.Domain.Company;

namespace Organization.Application.Commons.CQRS.CompanyModule.Queries
{
    public record GetCompaniesQuery(CompanyQueryParameters queryParameters): IRequest<PageList<CompanyResponseDto>>;
   
}
