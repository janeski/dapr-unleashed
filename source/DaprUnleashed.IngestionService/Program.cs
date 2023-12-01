using DaprUnleashed.API;
using DaprUnleashed.API.Services.Implementations;
using DaprUnleashed.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureAppSettings();
builder.Services.AddApplicationInsightsTelemetry();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDaprClient();

builder.Services.AddSingleton<IPromtService, PromtService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();