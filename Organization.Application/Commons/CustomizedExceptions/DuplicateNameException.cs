using Organization.Application.Commons.Interfaces;
using System.Net;

namespace Organization.Application.Commons.CustomizedExceptions
{
    public sealed class DuplicateNameException : Exception, IApplicationException
    {
        public DuplicateNameException(string errorMessage) : base(errorMessage)
        {

        }

        public HttpStatusCode StatusCode => HttpStatusCode.Conflict;

        public string ErrorMessage => Message;
    }

}
