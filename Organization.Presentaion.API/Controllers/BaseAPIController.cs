using ErrorOr;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Organization.Application.Commons.Utilities;

namespace Organization.Presentaion.API.Controllers
{
    public class BaseAPIController : ControllerBase
    {
        //NOTE: this ProblemFromErrors method MUST be "protected"
        //so that ProblemFromErrors method can be accessed by the derived classes (e.g. EmployeesController, CompaniesController)
        protected IActionResult GetProblemFromErrorsCollection(List<Error> errors)
        {
            if (errors.Count is 0)
            {
                //ObjectResult.ControllerBase.Problem()
                //if COUNT = 0, then return the ERRORs in the Microsoft.aspnetcore.Mvc's Problem( )
                return Problem();  //create objectResult for the response
            }

            //else we have MULTIPLE Errors &
            //check if EACH error type is a "VALIDATION" type
            if (errors.All(error => error.Type == ErrorType.Validation))
            {
                //if All errors are of type "Validation",
                //then add each error to ModelStateDictionary & return ValidationProblem(ModelStateDictionary)
                return AddEachErrorToModelStateDictionary(errors);
            }

            //else get & set key/value errors collection of HttpContext.Items named "errors"
            //HttpContext.Items["errors"] = errors;
            HttpContext.Items[GlobalConstants.Errors] = errors;

            return GetErrorsDetails(errors[0]);  //return 1st error in the errors collecton
        }

        private IActionResult GetErrorsDetails(Error error)
        {
            var statusCode = error.Type switch
            {
                //swith - case statements to check the error.Type & return appropriate StatusCode 
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.Validation => StatusCodes.Status400BadRequest,
                ErrorType.NotFound => StatusCodes.Status404NotFound,

                //Default option as "catch-all" for any other error types not specified above
                _ => StatusCodes.Status500InternalServerError
            };

            //this is the ObjectResult "Problem( )" method which Microsoft.aspnetcore.Mvc provides
            return Problem(detail: error.Code, statusCode: statusCode, title: error.Description);
        }

        private IActionResult AddEachErrorToModelStateDictionary(List<Error> errors)
        {
            //from Microsoft.AspNetCore.Mvc.ModelBinding
            var modelStateDictionary = new ModelStateDictionary();

            //takes modelstateDictionary & check each ERROR and add 
            //them to ModelStateDictionary accordly to "Code" & "Description"
            foreach (Error error in errors)
            {
                modelStateDictionary.AddModelError(error.Code, error.Description);
            }

            //then RETURN erros to ObjectResult's ValidationProblem which MicroSoft.aspnetcore.mvc provides
            return ValidationProblem(modelStateDictionary);
        }
    }
}
