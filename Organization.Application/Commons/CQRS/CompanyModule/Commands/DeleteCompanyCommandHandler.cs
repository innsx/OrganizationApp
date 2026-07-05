using ErrorOr;
using MediatR;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Commons.Errors;

namespace Organization.Application.Commons.CQRS.CompanyModule.Commands
{
    //MediatR.Unit Represent a VOID type, since VOID is not a VALID return type in C#.
    // we use "Unit" to represent a VOID return type in MediatR.
    public sealed class DeleteCompanyCommandHandler : IRequestHandler<DeleteCompanyCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCompanyCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(DeleteCompanyCommand request, CancellationToken cancellationToken)
        {
            var companyToSoftDelete = await _unitOfWork.Companies.GetByIdAsync(request.Id);
            //var companyToSoftDelete = await companyRepository.GetByIdAsync(id);

            if (companyToSoftDelete is null)
            {
                //return NotFound($"Company with Id: {id} not found.");

                //add this line
                //throw new CompanyNotFoundException($"The system does not have any Company with id = {request.Id}");
                var errorMessage = Errors.Company.CompanyFailToDelete($"Company with id: {request.Id} failed to SoftDelete.");

                return errorMessage;
            }

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            await _unitOfWork.Companies.SoftDeleteAsync(request.Id, request.isDeleteHasAssociations);
            //await companyRepository.SoftDeleteAsync(id, isSoftDeleteRecordHasRelatedChildTableColumn);

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            //since we CANNOT USE VOID as a RETURN type, we returns "Unit"
            return Unit.Value;  //returns Unit.Value as a VOID


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
