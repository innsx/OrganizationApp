using ErrorOr;
using MediatR;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.Interfaces.Persistance;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    public sealed class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand, ErrorOr<Unit>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ErrorOr<Unit>> Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeToUpdate = await _unitOfWork.Employees.GetByIdAsync(request.Id);

            if (employeeToUpdate == null)
            {
                //1st approach: return an error
                //throw new EmployeeNotFoundException($"Employee with id = {request.Id} is NOT found.");

                //2nd approach: using ErrorOr & returns customized error
                return Error.NotFound(
                    code: "Employee.NotFound",
                    description: $"Employee with id = {request.Id} is NOT found.");
            }
          
            employeeToUpdate.Name = request.Name;
            employeeToUpdate.Age = request.Age;
            employeeToUpdate.Position = request.Position;
            employeeToUpdate.Salary = request.Salary;
            employeeToUpdate.CreatedOn = employeeToUpdate.CreatedOn;
            employeeToUpdate.ModifiedOn = DateTime.Now;
            employeeToUpdate.CompanyId = request.CompanyId;

            _unitOfWork.OpenConnectionAndBeginDbTransaction();

            await _unitOfWork.Employees.UpdateAsync(employeeToUpdate);

            _unitOfWork.CommitDbTransactionDisposeAndCloseConnectionDispose();

            //since we CANNOT USE VOID as a RETURN type, we use "Unit"
            return Unit.Value; //returns Unit.Value as a VOID
        }
    }
}
