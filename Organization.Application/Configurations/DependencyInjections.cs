using Microsoft.Extensions.DependencyInjection;

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

            return services;
        }
    }
}
