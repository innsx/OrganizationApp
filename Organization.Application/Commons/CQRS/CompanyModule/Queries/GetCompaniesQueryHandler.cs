using ErrorOr;
using MediatR;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Company;

namespace Organization.Application.Commons.CQRS.CompanyModule.Queries
{
    public sealed class GetCompaniesQueryHandler : IRequestHandler<GetCompaniesQuery, ErrorOr<PageList<CompanyResponseDto>>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompaniesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<PageList<CompanyResponseDto>>> Handle(GetCompaniesQuery request, CancellationToken cancellationToken)
        {
            var companies = await _unitOfWork.Companies.GetCompaniesByQueryAsync(request.queryParameters);

            return companies;
        }
    }
}
