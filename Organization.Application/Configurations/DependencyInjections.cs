using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Organization.Application.Commons.PipelineBehaviours;

namespace Organization.Application.Configurations
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            //Registering AddMediator
            services.AddMediatR(config =>
            {
                //RegisterServicesFromAssembly( ) is part of the MediatR library's
                //Dependency Injection configuration.
                //It scans a specified assembly to automatically register all of your request handlers,
                //notification handlers,
                //and pipeline behaviors with the built-in .NET dependency injection container
                config.RegisterServicesFromAssembly(typeof(DependencyInjections).Assembly);
            });
             
            //tutorial: https://dotnettutorials.net/lesson/fluent-api-async-validators-in-asp-net-core-web-api/
            //REGISTERING A GENERIC VALIDATION CLASS 
            // ValidationPipelineBehaviour<TRequest, TResponse>
            //THAT IMPLEMENTS IPipelineBehavior INTERFACE
            //we are using GENERIC class here, so we need to specify the Generic Type Parameter "<,>"
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

            // IF Registering Each Validator Manually
            //builder.Services.AddScoped<IValidator<ProductDTO>, ProductDTOValidator>();
            //builder.Services.AddScoped<IValidator<CustomerDTO>, CustomerDTOValidator>();

            //Using Reflection, we Registering all Validators which are available
            // in this DependencyInjections class & within current running Assembly 
            services.AddValidatorsFromAssembly(typeof(DependencyInjections).Assembly);

            return services;
        }
    }
}
