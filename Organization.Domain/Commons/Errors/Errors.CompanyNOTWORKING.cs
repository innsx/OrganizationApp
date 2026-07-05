using ErrorOr;

namespace Organization.Domain.Commons.Errors
{
    public static partial class Errors
    {
        public static class CompanyNOTWORKING
        {
            public static Error DuplicateCompany(string msg) =>
                Error.Conflict(code: "ERR000_DUPLICATE_COMPANY", description: msg ?? "Company already exists in the system.");

            //other extension methods we can use
            //Error.Custom(...);
            //Error.Failure(...);
            //...

            public static Error CompanyDoestNotExist(string msg) =>
                Error.NotFound(code: "ERR001_COMPANY_NOTFOUND", description: msg ?? "Company does not exits in the system.");

            public static Error CompanyFailToDelete(string msg) =>
                Error.Failure(code: "ERR002_COMPANY_FAIL_TO_SOFTDELETE", description: msg ?? "Company failed to softDelete.");

            public static Error FailToAddCompany(string msg) =>
                Error.Failure(code: "ERR003_FAIL_TO_ADD_COMPANY", description: msg ?? "Company failed to add.");

            public static Error CompanyNameIsNullOrEmpty(string msg) =>
                Error.Failure(code: "ERR004_COMPANY_NAME_IS_NULL_OR_EMPTY", description: msg ?? "Company Name is null or empty.");
                        
        }
    }

}
