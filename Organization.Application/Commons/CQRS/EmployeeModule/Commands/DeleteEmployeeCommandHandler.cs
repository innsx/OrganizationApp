using ErrorOr;
using MediatR;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Commons.Errors;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    public sealed class DeleteEmployeeCommandHandler : IRequestHandler<DeleteEmployeeCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(DeleteEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeToDelete = await _unitOfWork.Employees.GetByIdAsync(request.Id);

            if (employeeToDelete == null)
            {
                //1st approach: use the NotFound() method in the controller to return a 404 response
                //(but this is not recommended because it will tightly couple
                //the handler with the controller)
                //return NotFound("employee is not found.");

                //2nd approach: creating a customized exception
                //then throw an error and catch it in the global exception handler middleware (in Program.cs)
                //throw new EmployeeNotFoundException($"The system does not have any Employee with id = {request.Id}");

                //3rd approach: create an error message in ErrorOr format
                //and return it to the controller class
                return Errors.Employee.EmployeeDoesNotExist($"The system does not have any Employee with id = {request.Id}"); 
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

                //since we CANNOT USE VOID as a RETURN type, we use "Unit"
                return Unit.Value; //returns Unit.Value as a VOID
            }
        }
    }
}
