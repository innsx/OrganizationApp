using Microsoft.Extensions.DependencyInjection;
using Organization.Application.Commons.ApplicationConfigOptions;
using Organization.Application.Commons.Interfaces.Authentications;
using Organization.Infrastructure.Authentications;
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
            //registering the DapperDataContext class as a scoped service in the dependency injection container.
            services.AddScoped<DapperDataContext>();


            //if a service is an OPTION object,
            //then we register the service using 
            //ConfigureOptions( )
            services.ConfigureOptions<JwtOptionsSetup>();

            //if a service is a CLASS,
            //then we register the service using 
            //1 of 3 services: AddScoped(), AddTranscent(), AddSingleton( )
            //registering the JwtTokenGenerator class as a singleton service in the dependency injection container.
            services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

            //if a service is an Authentication,
            //then we register the service using AddAuthentication().AddJwtBearer()
            services.AddAuthentication().AddJwtBearer();

            //if a service is an OPTION object, 
            //then we register the service using
            //ConfigureOptions( )
            services.ConfigureOptions<JwtBearerOptionsSetup>();

            return services;
        }
    }
}
