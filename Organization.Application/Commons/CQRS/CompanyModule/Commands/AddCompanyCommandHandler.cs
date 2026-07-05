using ErrorOr;
using MediatR;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Company.Models;
using Organization.Domain.Commons.Errors;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    public sealed class AddCompanyCommandHandler : IRequestHandler<AddCompanyCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public AddCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<ErrorOr<Unit>> Handle(AddCompanyCommand addCompanyRequest, CancellationToken cancellationToken)
        {
            bool isCompanyRequestedExisted = await _unitOfWork.Companies.IsExistingAsync(addCompanyRequest.Name!);

            if (isCompanyRequestedExisted is true)
            {
                //WILL RETURN STATUSCODE: 409 conflict if Name existed
                //return Conflict(companyRequestDto);

                //add this line and use DuplicateCompanyException with pass-in specified Company Name if Name is NOT UNIQUE
                //throw new DuplicateNameException($"Company with Name {addCompanyRequest.Name} is ALREADY EXISTED.");

                return Errors.Company.DuplicateCompany($"Company with Name {addCompanyRequest.Name} is ALREADY EXISTED.");
            }
             
            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            var newCompanyId = await _unitOfWork.Companies.AddAsnyc(new Company
            {
                Name = addCompanyRequest.Name,
                Address = addCompanyRequest.Address,
                Country = addCompanyRequest.Country,
            });

            //var newCompanyId = await companyRepository.AddAsnyc(new Company
            //{
            //    Name = companyRequestDto.Name,
            //    Address = companyRequestDto.Address,
            //    Country = companyRequestDto.Country,
            //});

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            //return newCompanyId;

            //since we CANNOT USE VOID as a RETURN type, we return "Unit"
            return Unit.Value;
        }
    }
}
