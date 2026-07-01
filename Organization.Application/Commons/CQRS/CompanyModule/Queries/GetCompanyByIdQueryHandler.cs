using MediatR;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Exceptions;
using Organization.Application.Commons.Interfaces.Persistance;

namespace Organization.Application.Commons.CQRS.CompanyModule.Queries
{
    public class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, CompanyResponseDto>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CompanyResponseDto> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {

            var requestCompany = await _unitOfWork.Companies.GetByIdAsync(request.Id);

            if (requestCompany is null)
            {
                throw new NotFoundException($"The system does not have any company by Id: {request.Id}");
            }

            return new CompanyResponseDto(requestCompany.Name!, requestCompany.Address!, requestCompany.Country!);


            //if (request.hasAssociatedObject is false)
            //{
            //    var company = await _unitOfWork.Companies.GetByIdAsync(request.Id);
            //    if (company == null)
            //    {
            //        //add this line
            //        throw new NotFoundException($"The system does not have any Company with id = {request.Id}");
            //    }

            //    return new CompanyResponseDto
            //    {
            //        Name = company.Name,
            //        Address = company.Address,
            //        Country = company.Country
            //    };
            //}
            //else
            //{
            //    //var company = await companyRepository.QueryOneToManyParentChildRelationshipAsync(id);

            //    var company = await _unitOfWork.Companies.QueryOneToManyParentChildRelationshipAsync(request.Id);

            //    if (company == null || company.Count == 0)
            //    {
            //        //add this line
            //        throw new NotFoundException($"The system does not have any Company with id = {request.Id}");

            //    }
            //    return company;

            //    //var c = new Company();
            //    //foreach (var item in company)
            //    //{
            //    //   c = new Company { Name = item.Name, Address = item.Address, Country = item.Country, Employees = item.Employees };
            //    //}

            //    //return c;
            //}
        }
    }
}
