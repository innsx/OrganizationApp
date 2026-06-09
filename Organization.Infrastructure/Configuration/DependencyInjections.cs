using Microsoft.Extensions.DependencyInjection;
using Organization.Infrastructure.Persistance.DataContext;

namespace Organization.Infrastructure.Configuration
{
    public static class DependencyInjections
    {
        //If you specified 'this' before the very first parameter of a static method,
        //you are creating an Extension Method for IServiceCollection
        //Without this: You must call it like a normal static method: builder.Services.AddInfrastructure(service)
        //With this: You can call it cleanly: builder.Services.AddInfrastructure()
        public static IServiceCollection AddInfastructure(this IServiceCollection services)
        {
            services.AddScoped<DapperDataContext>();

            return services;
        }
    }
}
