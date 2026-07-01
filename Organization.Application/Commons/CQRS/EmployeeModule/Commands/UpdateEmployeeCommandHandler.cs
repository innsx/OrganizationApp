using MediatR;
using Organization.Application.Commons.Exceptions;
using Organization.Application.Commons.Interfaces.Persistance;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Commands
{
    public sealed class UpdateEmployeeCommandHandler : IRequestHandler<UpdateEmployeeCommand>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateEmployeeCommandHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(UpdateEmployeeCommand request, CancellationToken cancellationToken)
        {
            var employeeToUpdate = await _unitOfWork.Employees.GetByIdAsync(request.Id);

            if (employeeToUpdate == null)
            {
                throw new NotFoundException($"Employee with id = {request.Id} is NOT found.");
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
        }
    }
}
