using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Organization.Application.Commons.Interfaces.Persistance;
using Organization.Application.Commons.Interfaces.Persistance.RepositoriesFactory;
using Organization.Infrastructure.Persistance;
using Organization.Infrastructure.Persistance.RepositoriesFactory;
using Organization.Presentaion.API.Common.Exceptions;
using Organization.Presentaion.API.Common.Mappings;
using Organization.Presentaion.API.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Organization.Presentaion.API.Configurations
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddPresentation(this IServiceCollection services)
        {
            //this line is to add the Controllers to the services collection
            services.AddControllers();

            //this line is to add the Endpoints API Explorer to the services collection
            services.AddEndpointsApiExplorer();

            //Commmented this line
            //& added the below lines to document in XML format
            //& Enable Authentication to get in Swagger when Swagger documentation gets load
            //services.AddSwaggerGen();

            //this line is to add the SwaggerGen to the services collection
            services.AddSwaggerGen(options =>
            {
                //& PROVIDES & SPECIFies the PATH to our XML file in the location of our Organisation Project:
                //D:\Repos\OrgAppZero2Prod\OrganisationApp\Organisation.Presentation.API\bin\Debug\net7.0
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));

                //we ENABLE Authentication to get in Swagger when Swagger documentation gets load
                options.AddSecurityDefinition("oauth2",
                                               new OpenApiSecurityScheme
                                               {
                                                   In = ParameterLocation.Header,
                                                   Name = "Authorization",
                                                   Type = SecuritySchemeType.ApiKey
                                               }
                );

                //this line is to add the SecurityRequirementsOperationFilter to the SwaggerGen options
                //that filters any security operations
                options.OperationFilter<Swashbuckle.AspNetCore.Filters.SecurityRequirementsOperationFilter>();
            });

            //this line is to add the ProblemDetailsFactory to the services collection
            services.AddScoped<IRepositoryFactory, RepositoryFactory>();

            //this is a API Versioning setup to work with SwaggerUI
            services.AddApiVersioning(options =>
            {
                
                // -----------------  OPTIONS: URL Versioning  ----------------------------------
                //If set to "TRUE", the response contains service API Versioning compatibility
                //in the API information response
                //DEFAULT VALUE IS SET TO "FALSE"
                options.ReportApiVersions = true;

                //Implementing URI API versioning
                //DEFAULT API version is SET to version 1 and NO minor version needed to SET it to
                options.DefaultApiVersion = new ApiVersion(1, 0);

                //take the 1st version, if NOT Specified a version at
                //this line above “options.DefaultApiVersion…”
                options.AssumeDefaultVersionWhenUnspecified = true;

                // ---- OPTION: if Implementing query string custom API versioning, use this line -------------
                //options.ApiVersionReader = new QueryStringApiVersionReader("organisationApp-api-version with Query String");

                // ---- OPTION: if Implementing header API versioning, use this line --------------------------
                //options.ApiVersionReader = new HeaderApiVersionReader("X-API-Version");
            });

            //configure Swagger to support API Versioning
            services.AddVersionedApiExplorer(options =>
            {
                //DEFAULT VALUE IS NULL
                //string format use to format API Version as a Group name
                options.GroupNameFormat = "'v'VVV";

                //DEFAULT VALUE IS set to FALSE
                //if set to TRUE as you see below,
                //the API Version parameters should be substitute in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            //Registering SwaggerGenOptions
            //WHEN setting up AddApiVersioning,
            //we MUST REGISTER/INJECT "<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>()"
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

            //Registering UnitOfWork with injecting IUnitOfWork interface
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //REGISTERING CustomProblemDetailsFactory with injecting System's ProblemDetailsFactory class
            services.AddSingleton<ProblemDetailsFactory, CustomProblemDetailsFactory>();

            //we call static AddMappingConfigurations( ) in DependencyInjections.cs Class
            //& REGISTER Global Mapping configurations with Mapster's "AddMappings" object
            services.AddMappingConfigurations();

            return services;
        }
    }
}