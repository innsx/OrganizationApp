using MediatR;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.Interfaces.Persistance;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    public sealed class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeToDelete = await _unitOfWork.Employees.GetByIdAsync(request.Id);

            if (employeeToDelete == null)
            {
                //add this line
                throw new EmployeeNotFoundException($"The system does not have any Employee with id = {request.Id}");
            }

            if (request.isDeleteHasAssociations is true)
            {
                throw new InvalidOperationException($"The Employee with id = {request.Id} has associated records and cannot be deleted.");
            }
            else
            {
                // Proceed with deletion
                bool isDeleteEmployeeHasAssociation = false;

                _unitOfWork.OpenConnectionAndBeginDbTransaction();

                await _unitOfWork.Employees.SoftDeleteAsync(request.Id, isDeleteEmployeeHasAssociation);

                _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();
            }
        }
    }
}
