using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Interfaces.Persistance.RepositoriesFactory;
using Organization.Infrastructure.Persistance;
using Organization.Infrastructure.Persistance.RepositoriesFactory;
using Organization.Presentaion.API.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Organization.Presentaion.API.Configurations
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            services.AddControllers();

            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen();

            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

            //this is a API Versioning setup to work with SwaggerUI
            services.AddApiVersioning(options =>
            {
                //get or set a value indicating API Versioning is compatible in the API information response
                options.ReportApiVersions = true;

                //Implementing URI API versioning
                //DEFAULT API version is SET to version 1 and NO minor version to SET to
                options.DefaultApiVersion = new ApiVersion(1, 0);

                //take the 1st version, if NOT Specified a version at this line above “options.DefaultApiVersion…”
                options.AssumeDefaultVersionWhenUnspecified = true;

                //Implementing query string custom API versioning
                //options.ApiVersionReader = new QueryStringApiVersionReader("organisationApp-api-version with Query String");
               
                //Implementing header API versioning
                //options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
            });

            //configure Swagger to support API Versioning
            services.AddVersionedApiExplorer(options =>
            {
                //DEFAULT VALUE IS NULL
                //string format use to format API Version as a Group name
                options.GroupNameFormat = "'v'VVV";

                //DEFAULT VALUE IS set to FALSE
                //if set to TRUE as you see below, the API Version parameters should be substitute in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            //Registering SwaggerGenOptions
            //WHEN setting up AddApiVersioning,
            //we MUST INJECT "<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()"
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();


            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
