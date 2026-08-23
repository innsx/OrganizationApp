using ErrorOr;
using FluentValidation;
using MediatR;
using Organization.Domain.Commons.Errors;

namespace Organization.Application.Commons.PipelineBehaviours
{
    //IPipelineBehavior is an interface from MediatR library that defines
    //a pipeline behavior for handling requests and responses in a mediator pattern.
    public class ValidationPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> 
            where TRequest : IRequest<TResponse>    //generic constraints
            where TResponse : IErrorOr //Constraint IErrorOr is from ErrorOr Library where ErrorOr Inherits IErrorOr
    {

        //add nullable _validator Field of type IValidator<TRequest>
        //to the ValidationPipelineBehaviour class
        private readonly IValidator<TRequest>? _validator;

        //Constructor Injection of IValidator<TRequest> into the ValidationPipelineBehaviour class
        public ValidationPipelineBehaviour(IValidator<TRequest>? validator = null)
        {
            _validator = validator;
        }


        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            // ----------------- BEFORE Handler class executes ------------------
            //if _validator is NULL, move on to the next item
            if (_validator is null)
            {
                await next();
            }

            //if _validator is NOT NULL, we validate the ''request object'' FIELDs
            var validationResult = await _validator.ValidateAsync(request, cancellationToken);


            // -------------- AFTER the HANDLER is executed, we check validationResult IsValid ---------------------
            if (validationResult.IsValid is true)
            {
                return await next(); // Proceed to the next middleware/handler
            }

            //converting all validationResults into Errors
            var errors = validationResult.Errors
                            .ConvertAll(
                                ValidationFailure => Error.Validation(
                                    code: ValidationFailure.ErrorCode ?? ValidationFailure.PropertyName,
                                    description: ValidationFailure.ErrorMessage
                                )
                            );

            //Option 1: Return a new instance of TResponse with the errors
            //return (TResponse)Activator.CreateInstance(typeof(TResponse), errors)!;

            //option 2: Return a dynamic object with the errors
            //"dynamic" represent an object which will RESOLVE OBJECT conversion at RUNTIME
            return (dynamic)errors;
        }
    }
}
