using FancyMicroservice.Extensions;
using FancyMicroservice.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHealthChecks()
    .AddCheck<LivenessHealthCheck>(
        "LivenessHealthCheck",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "liveness" }
    )
    .AddCheck<ReadinessHealthCheck>(
        "ReadinessHealthCheck",
        failureStatus: HealthStatus.Unhealthy,
        tags: new[] { "readiness" }
    );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// In this example HTTPS is disabled.
// app.UseHttpsRedirection();

app.MapHealthChecks();

app.UseAuthorization();

app.MapControllers();

app.Run();
