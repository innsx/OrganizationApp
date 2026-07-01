using Mapster;
using MapsterMapper;

namespace Organization.Presentaion.API.Common.Mappings
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddMappingConfigurations(this IServiceCollection services)
        {
            //creating a variable of type "TypeAdapterConfig.GlobalSettings"
            var typeAdapterConfig = TypeAdapterConfig.GlobalSettings;

            //Using the config variable, we do a SCAN of the "Assemblies" with using REFLECTION
            // & check where "GlobalMappingConfig.cs" it located
            // & reads ALL CONFIGURATIONS setup in this class
            // & then it makes sure ALL GLOBAL MAPPINGS ARE LOADED
            typeAdapterConfig.Scan(typeof(DependencyInjections).Assembly);

            //then we INJECT typeAdapterConfig variable as a SINGLETON DEPENDENCY OBJECT of "GlobalMappingConfig.cs"
            services.AddSingleton(typeAdapterConfig);

            //we INJECT IMapper, ServiceMapper as a DEPENDENCY OBJECT which we will INJECT
            //these OBJECTS in our CONTROLLER Classes or any where this object is NEEDED
            // ServiceMapper is an WRAPPER object that is part of MAPster Library
            // which ServiceMapper includes objects for DEPENDENCY INJECTION
            services.AddScoped<IMapper, ServiceMapper>();

            return services;
        }
    }
}
