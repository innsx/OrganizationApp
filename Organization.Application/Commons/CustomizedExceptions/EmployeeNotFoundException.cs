using Organization.Application.Commons.Interfaces;
using System.Net;

namespace Organization.Application.Commons.CustomizedExceptions
{
    public class EmployeeNotFoundException : Exception, IApplicationException
    {
        public EmployeeNotFoundException(string errorMessage) : base(errorMessage)
        {
        }

        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;
        public string ErrorMessage => Message;

    }
}
