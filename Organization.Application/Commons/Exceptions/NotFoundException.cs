using Organization.Application.Commons.Interfaces;
using System.Net;

namespace Organization.Application.Commons.Exceptions
{
    public sealed class NotFoundException : Exception, IApplicationException
    {
        public NotFoundException(string errorMessage) : base(errorMessage)
        {

        }

        public HttpStatusCode StatusCode => HttpStatusCode.NotFound;

        public string ErrorMessage => Message;
    }

}
