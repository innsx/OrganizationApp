using MediatR;
using Organization.Application.Commons.Interfaces.Persistance;

namespace Organization.Application.Commons.CQRS.CompanyModule.Queries.GetCompanyCount
{
    public class GetCountQueryHandler : IRequestHandler<GetCompanyCountQuery, int>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCountQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<int> Handle(GetCompanyCountQuery request, CancellationToken cancellationToken)
        {
            //here we're not using Generic Factory Repository pattern
            return await _unitOfWork.Companies.GetTotalCountAsync();
        }
    }
}
