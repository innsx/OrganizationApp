using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Organization.Application.Commons.Utilities;
using Organization.Application.Configurations;
using Organization.Infrastructure.Configuration;
using Organization.Presentaion.API.Configurations;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

// Read Serilog config from appsettings.json & instantiate a configBuilderAppsettingsFile
var configBuilderAppsettingsFile = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();

//instantiate a loggerConfigFile and read configBuilderAppsettingsFile and create a log
Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configBuilderAppsettingsFile).CreateLogger();


//If the JSON configuration continues to fail,
//you can bypass the JSON configuration error by
//configuring the sink directly in your code during startup.
#pragma warning disable
//Log.Logger = new LoggerConfiguration()
//    .ReadFrom.Configuration(configBuilderAppsettingsFile)
//    .WriteTo.MSSqlServer(
//        connectionString: configBuilderAppsettingsFile.GetConnectionString("SqlConnection"),
//        sinkOptions: new Serilog.Sinks.MSSqlServer.MSSqlServerSinkOptions
//        {
//            TableName = "Logs",
//            AutoCreateSqlTable = true
//        }
//    )
//    .CreateLogger();

//to make sure our Organization.Presentation.API starts or NOT START due to ANY ERROR
//we add a TRY-CATCH blck to log either EVENTs
try
{
    //Add Log information about when our Organization.Presentation.API is STARTED
    Log.Information("{ApplicationName} is starting up.", GlobalConstants.ApplicationName);

    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services
        .AddApplication()
        .AddPresentation()
        .AddInfastructure();

    var app = builder.Build();

    //we needed to Get a SERVICE of the TYPE IApiVersionDescriptionProvider
    var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        // commented this line so we can specifies a Swagger with options
        //app.UseSwaggerUI();  

        //adding more Swagger UI Options
        app.UseSwaggerUI(c =>
        {
            foreach (var description in provider.ApiVersionDescriptions)
            {
                c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());

                //c.RoutePrefix = "api/documentation"; //Adds a PREFIX to SWAGGER's ROUTE
                c.DefaultModelExpandDepth(2);
                c.DocExpansion(DocExpansion.List);  //List: lists & expands ALL ENDPOINT (is the DEFAULT)  when loaded
                                                    //c.DocExpansion(DocExpansion.Full);  //Full: expands ALL ENDPOINT (is the DEFAULT)  when loaded
                                                    //c.DocExpansion(DocExpansion.None);    //None: will not expand the ENDPOINTS when loaded
                c.DisplayRequestDuration();
            }
        });

    }

    // we will add this line as a middleware PIPELINE
    // & every time an ERROR occurred,  
    // ErrorController.cs Class’s Error( ) Endpoint will get HITTED
    // & the Error( ) will catch ALL EXCEPTIONS & LOGGED THOSE EXCEPTIONS
    app.UseExceptionHandler("/Error");

    app.UseHttpsRedirection();

    //Setup Serilog in MIDDLEWARE 
    app.UseSerilogRequestLogging();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    //this is STRUCTURE LOGGING setup
    Log.Fatal(ex, "{ApplicationName} failed to start up.", GlobalConstants.ApplicationName);
}
finally
{
    Log.CloseAndFlush();
}

