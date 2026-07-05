using Organization.Application.Commons.Interfaces;
using System.Net;

namespace Organization.Application.Commons.CustomizedExceptions
{
    public sealed class CompanyNotFoundException : Exception, IApplicationException
    {
        public CompanyNotFoundException(string errorMessage) : base(errorMessage)
        {

        }

        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }

}
