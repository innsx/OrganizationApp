using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Organization.Application.Commons.Interfaces;

namespace Organization.Presentaion.API.Controllers
{
    [Route("/Error")]
    public sealed class ErrorController : ControllerBase
    {       
        public IActionResult Error()
        {
            var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

            //c#9.0 switch - case statements
            var (statusCode, message) = exception switch
            {
                IApplicationException appException => (Convert.ToInt32(appException.StatusCode), appException.ErrorMessage),

                //switch - case DEFAULT statement
                _ => (StatusCodes.Status500InternalServerError, "An Unexpected error occurred")
            };

            return Problem(statusCode: statusCode, title: message);

        }
    }
}
