using System.Net;

namespace Organization.Application.Commons.Interfaces
{
    public interface IApplicationException
    {
        //NOTE: we needed to GET only, and if you specified with Set, you will get Error
        public HttpStatusCode StatusCode { get; }
        public string ErrorMessage { get; }

    }
}
