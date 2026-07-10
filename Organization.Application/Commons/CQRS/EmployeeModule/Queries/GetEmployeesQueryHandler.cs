using ErrorOr;
using MediatR;
using Organization.Application.Commons.DTOs;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Utilities;

namespace Organization.Application.Commons.CQRS.EmployeeModule.Queries
{
    public sealed class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, PageList<EmployeeResponseDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetEmployeesQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PageList<EmployeeResponseDto>> Handle(GetEmployeesQuery request, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Employees.GetEmployeesByQueryAsync(request.employeeQueryParameters);
        }
    }
}
