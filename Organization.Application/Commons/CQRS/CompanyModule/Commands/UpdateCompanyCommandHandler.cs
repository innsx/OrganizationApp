using ErrorOr;
using MediatR;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Commons.Errors;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    //MediatR.Unit Represent a VOID type, since VOID is not a VALID return type in C#.
    // we use "Unit" to represent a VOID return type in MediatR.
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var updateCompany = await _unitOfWork.Companies.GetByIdAsync(request.Id);

            if (updateCompany is null)
            {
                //throw new CompanyNotFoundException($"The system does not have any company with id={request.Id}");
                var errorMessage = Errors.Company.CompanyDoestNotExist($"The system does not have any company with id = {request.Id}");
                return errorMessage;

            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            updateCompany.Name = request.Name;
            updateCompany.Address = request.Address;
            updateCompany.Country = request.Country;

            await _unitOfWork.Companies.UpdateAsync(updateCompany);

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            //since we CANNOT USE VOID as a RETURN type, we use "Unit"
            return Unit.Value; //returns Unit.Value as a VOID
        }
    }
}
