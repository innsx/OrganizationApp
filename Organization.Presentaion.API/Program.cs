using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Organization.Application.Configurations;
using Organization.Infrastructure.Configuration;
using Organization.Presentaion.API.Configurations;

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
        }
    });

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
