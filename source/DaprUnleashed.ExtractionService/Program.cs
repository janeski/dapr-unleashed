
using DaprUnleashed.ExtractionService.Services.Implementations;
using DaprUnleashed.ExtractionService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<IExtractionService, ExtractionService>();
builder.Services.AddControllers().AddDapr();
builder.Services.AddDaprClient();

var app = builder.Build();

app.UseAuthorization();
app.UseCloudEvents();
app.MapControllers();
app.MapSubscribeHandler();
app.Run();