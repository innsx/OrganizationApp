using System.Net;

namespace Organization.Application.Commons.Interfaces
{
    public interface IApplicationException
    {
        /*
         NOTE: if you specified with “set”, you will get Error 
            because when you try to change its value from outside its own class, 
            C# blocks outside access on purpose to keep your data safe and 
            stop other code from breaking your object's rules or 
                when external frameworks like serializers cannot access the private setter
         */
        public HttpStatusCode StatusCode { get; }
        public string ErrorMessage { get; }

    }
}
