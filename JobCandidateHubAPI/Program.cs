using JobCandidateHubAPI;
using JobCandidateHubAPI.Dtos.Candidates;
using JobCandidateHubAPI.Implementations;
using JobCandidateHubAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using FluentValidation;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//add the database context to the services collection
builder.Services.AddDbContext<JobCandidateDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ICandidateService, CandidateService>();

builder.AddFluentValidationEndpointFilter();
builder.Services.AddValidatorsFromAssemblyContaining<CandidateValidator>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    //TODO not recommend for production use case
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<JobCandidateDbContext>();
        context.Database.EnsureCreated();
        context.Database.Migrate();
    }
    app.MapOpenApi();
    app.MapScalarApiReference();
    //Configure by default to open scalar api reference

}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () =>
        Results.Redirect("/scalar")).ExcludeFromDescription();
}

app.MapPost("/candidate",
    async (ICandidateService candidateService,[FromBody] CreateOrUpdateCandidateRequestInput requestInput) =>
    {
        //add modal validation

        var result = await candidateService.CreateOrUpdate(requestInput);
        if (result.IsUpdate)
        {
            return Results.Ok();
        }
        return Results.Created();

    }).AddFluentValidationFilter();

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

internal record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
