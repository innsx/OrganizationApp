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

            //REGISTERING the TYPE: IPipelineBehavior & IMPEMENTATION TYPE:ValidationPipelineBehaviour
            //since in ValidationPipelineBehaviour.cs class, we are using GENERIC 
            //here we need to specifie Generic Type Parameter "<,>"
            services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehaviour<,>));

            //Using Reflection, we Registering all Validators which are available
            // within current running Assembly
            services.AddValidatorsFromAssembly(typeof(DependencyInjections).Assembly);

            return services;
        }
    }
}
