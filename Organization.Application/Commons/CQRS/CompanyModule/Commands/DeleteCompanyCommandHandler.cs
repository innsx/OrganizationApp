using MediatR;
using Organization.Application.Commons.Exceptions;
using Organization.Application.Commons.Interfaces.Persistance;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    public sealed class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var companyToSoftDelete = await _unitOfWork.Companies.GetByIdAsync(request.Id);
            //var companyToSoftDelete = await companyRepository.GetByIdAsync(id);

            if (companyToSoftDelete is null)
            {
                //return NotFound($"Company with Id: {id} not found.");

                //add this line
                throw new NotFoundException($"The system does not have any Company with id = {request.Id}");
            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            await _unitOfWork.Companies.SoftDeleteAsync(request.Id, request.isDeleteHasAssociations);
            //await companyRepository.SoftDeleteAsync(id, isSoftDeleteRecordHasRelatedChildTableColumn);

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            //if (request.isDeleteHasAssociations == true)
            //{
            //    return $"Company with Id: {request.Id} is successfully Soft-Deleted in Parent Table column and Child Table column";
            //}
            //else
            //{
            //    return $"Company with Id: {request.Id} is successfully Soft-Deleted in Parent Table column";
            //}
        }
    }
}
