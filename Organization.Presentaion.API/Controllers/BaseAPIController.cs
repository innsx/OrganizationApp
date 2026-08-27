using ErrorOr;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Organization.Application.Commons.Utilities;
using Organization.Domain.Commons.Errors.CustomsEnums;

namespace Organization.Presentaion.API.Controllers
{
    [Authorize]
    public class BaseAPIController : ControllerBase
    {
        //NOTE: this ProblemFromErrors method MUST be "protected"
        //so that ProblemFromErrors method can be accessed by the derived classes (e.g. EmployeesController, CompaniesController)
        protected IActionResult GetProblemFromErrorsCollection(List<Error> errors)
        {
            if (errors.Count is 0)
            {
                //ObjectResult.ControllerBase.Problem()
                //if COUNT = 0, then return the ERRORs from Microsoft.aspnetcore.Mvc's
                //Problem( ) method which is a default ObjectResult for the response
                return Problem();  
            }

            //if we have MULTIPLE Errors &
            //check if EACH error type is a "VALIDATION" type
            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                //if All errors are of type "Validation",
                //then add each error to ModelStateDictionary & return ValidationProblem(ModelStateDictionary)
                return AddErrorCollectionInToModelStateDictionary(errors);
            }

            //else get & set key/value errors collection of HttpContext.Items named "errors"
            //HttpContext.Items["errors"] = errors;
            HttpContext.Items[GlobalConstants.Errors] = errors;

            return GetErrorsDetails(errors[0]);  //return 1st error in the errors collecton
        }

        private IActionResult GetErrorsDetails(Error error)
        {
            //switch statement to determine the appropriate HTTP status code based on the error type
            var statusCode = (int)error.Type switch
            {
                //swith - case statements check the error.Type & return appropriate StatusCode                 
                (int)ErrorType.Conflict => StatusCodes.Status409Conflict,
                (int)ErrorType.Validation => StatusCodes.Status400BadRequest,
                (int)ErrorType.NotFound => StatusCodes.Status404NotFound,

                (int)CustomEnumWithErrorTypes.UnAuthorized => StatusCodes.Status401Unauthorized,
                (int)CustomEnumWithErrorTypes.Forbidden => StatusCodes.Status403Forbidden,


                //Default option as "catch-all" for any other error types not specified above
                _ => StatusCodes.Status500InternalServerError
            };

            //this is the ObjectResult default "Problem( )" method which Microsoft.aspnetcore.Mvc provides
            return Problem(detail: error.Code, statusCode: statusCode, title: error.Description);
        }


        private IActionResult AddErrorCollectionInToModelStateDictionary(List<Error> errors)
        {
            //from Microsoft.AspNetCore.Mvc.ModelBinding
            var modelStateDictionary = new ModelStateDictionary();

            //takes modelstateDictionary & check each ERROR and add 
            //them to ModelStateDictionary accordly to "Code" & "Description"
            foreach (Error error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }

            //then RETURN errors to ObjectResult's ValidationProblem which MicroSoft.aspnetcore.mvc provides
            return ValidationProblem(modelStateDictionary);
        }
    }
}
