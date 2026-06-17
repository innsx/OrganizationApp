using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Organization.Application.Configurations;
using Organization.Infrastructure.Configuration;
using Organization.Presentaion.API.Configurations;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
