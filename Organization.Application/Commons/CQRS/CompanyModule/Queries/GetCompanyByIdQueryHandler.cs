using MediatR;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.Interfaces.Persistance;
using ErrorOr;
using Organization.Domain.Commons.Errors;

namespace Organization.Application.Commons.CQRS.CompanyModule.Queries
{
    public sealed class GetCompanyByIdQueryHandler : IRequestHandler<GetCompanyByIdQuery, ErrorOr<CompanyResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCompanyByIdQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<CompanyResponseDto>> Handle(GetCompanyByIdQuery request, CancellationToken cancellationToken)
        {            
            var requestCompany = await _unitOfWork.Companies.GetByIdAsync(request.Id);

            
            if (requestCompany is null)
            {
                //1st approach: use the NotFound() method in the controller to return a 404 response
                //(but this is not recommended because it will tightly couple
                //the handler with the controller)
                //return NotFound(employee);

                //2nd approach: creating a customized exception
                //and throwing it to the controller class
                //throw new CompanyNotFoundException($"The system does not have any company by Id: {request.Id}");

                //3rd approach: create an error message in ErrorOr format
                //and return it to the controller class
                var errorMessage = Errors.Company.CompanyDoestNotExist($"The system does not have any company with id = {request.Id}");

                return errorMessage;
            } 

            return new CompanyResponseDto(requestCompany.Name!, requestCompany.Address!, requestCompany.Country!);

        }
    }
} 





//2nd approach
//if (requestCompany is null)
//{
//    throw new CompanyNotFoundException($"The system does not have any company by Id: {request.Id}");
//}

//return new CompanyResponseDto(requestCompany.Name!, requestCompany.Address!, requestCompany.Country!);



//1st approach
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