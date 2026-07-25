using Organization.Application.Commons.Interfaces;
using System.Net;

namespace Organization.Application.Commons.CustomizedExceptions
{
    public sealed class CompanyNotFoundException : Exception, IApplicationException
    {
        public CompanyNotFoundException(string errorMessage) : base(errorMessage)
        {

        }

        //This syntax defines a read-only property named StatusCode using expression body definition (=>),
        //  returning the HttpStatusCode Enum value NotFound (HTTP 404).
        //It is commonly used in custom exception classes,
        //  API result wrappers, or
        //  error models to hardcode or
        //  signal a missing resource state
        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }

}
