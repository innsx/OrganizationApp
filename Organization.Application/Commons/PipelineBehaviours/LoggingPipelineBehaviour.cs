using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Organization.Application.Commons.PipelineBehaviours
{
    public sealed class LoggingPipelineBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
        where TResponse : IErrorOr
    {
        private readonly ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> _logger;

        public LoggingPipelineBehaviour(ILogger<LoggingPipelineBehaviour<TRequest, TResponse>> logger)
        {
            //mapping
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
        {
            _logger.LogInformation(
                "Starting {RequestName} request at {DateTimeUTC}",
                typeof(IRequest).Name,
                DateTime.UtcNow
            );

            var result = await next();

            if (result.IsError)
            {
                _logger.LogError(
                    "Request {RequestName} request at {DateTimeUTC}, {Error}",
                    typeof(IRequest).Name, DateTime.UtcNow,
                    result.Errors?.Select(e => e)
                );
            }

            _logger.LogInformation(
                "Completed {RequestName} request at {DateTimeUTC}",
                typeof(IRequest).Name,
                DateTime.UtcNow
            );

            return result;
        }
    }
}