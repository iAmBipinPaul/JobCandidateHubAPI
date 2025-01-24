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
}

app.UseHttpsRedirection();

if (app.Environment.IsDevelopment())
{
    app.MapGet("/", () =>
        Results.Redirect("/scalar")).ExcludeFromDescription();
}

app.MapPost("/candidate",
    async (ICandidateService candidateService,[FromBody] CreateOrUpdateCandidateRequestInput requestInput) =>
    {
        var result = await candidateService.CreateOrUpdate(requestInput);
        if (result.IsUpdate)
        {
            return Results.Ok();
        }
        return Results.Created();

    }).AddFluentValidationFilter();


app.MapGet("/candidates", async (ICandidateService candidateService) =>await candidateService.GetCandidates());
app.Run();

