using ErrorOr;
using MediatR;
using Organization.Application.Commons.CustomizedExceptions;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Domain.Commons.Errors;

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
                //1st approach: use the NotFound() method in the controller to return a 404 response
                //(but this is not recommended because it will tightly couple
                //the handler with the controller)
                //return NotFound("employee is not found.");

                //2nd approach: creating a customized exception
                //then throw an error and catch it in the global exception handler middleware (in Program.cs)
                //throw new EmployeeNotFoundException($"Employee with id = {request.Id} is NOT found.");

                //3rd approach: create an error message in ErrorOr format
                //and return it to the controller class  
                return Errors.Employee.EmployeeDoesNotExist($"Employee with id = {request.Id} does not existed.");
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
