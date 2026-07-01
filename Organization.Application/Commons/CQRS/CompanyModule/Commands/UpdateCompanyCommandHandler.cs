using MediatR;
using Organization.Application.Commons.Exceptions;
using Organization.Application.Commons.Interfaces.Persistance;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    public class UpdateCompanyCommandHandler : IRequestHandler<UpdateCompanyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateCompanyCommand request, CancellationToken cancellationToken)
        {
            var updateCompany = await _unitOfWork.Companies.GetByIdAsync(request.Id);

            if (updateCompany is null)
            {
                throw new NotFoundException($"The system does not have any company with id={request.Id}");
            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            updateCompany.Name = request.Name;
            updateCompany.Address = request.Address;
            updateCompany.Country = request.Country;

            await _unitOfWork.Companies.UpdateAsync(updateCompany);

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();
        }
    }
}
