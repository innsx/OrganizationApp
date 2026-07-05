using ErrorOr;

namespace Organization.Domain.Commons.Errors
{
    public static partial class Errors
    {
        public static class EmployeeNOTWORKING
        {
            public static Error DuplicateEmployee(string msg) =>
                Error.Conflict(code: "ERR_EMPLOYEE_DUPLICATE", description: msg ?? "Employee already exists in the system.");

            public static Error EmployeeDoesNotExist(string msg) =>
                Error.NotFound(code: "ERR_EMPLOYEE_NOTFOUND", description: msg ?? "Employee does not exits in the system.");


            public static Error EmployeeFailToDelete(string msg) =>
                Error.Failure(code: "ERR_EMPLOYEE_FAIL_TO_SOFTDELETE", description: msg ?? "Employee failed to softDelete.");


            public static Error SalaryLessThanOrEqualToZero(string msg) =>
                Error.Failure(code: "ERR_SALARY_LESSTHAN_OR_EQUAL_ZERO", description: msg ?? "Employee Salary is Less than or equal Zero.");


            public static Error FailToAddEmployee(string msg) =>
                Error.Failure(code: "ERR_FAIL_TO_ADD_EMPLOYEE", description: msg ?? "Employee failed to add.");
        }

    }
