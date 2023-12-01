using DaprUnleashed.TransformationService.Services.Implementations;
using DaprUnleashed.TransformationService.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ITransformationService, TransformationService>();
builder.Services.AddControllers().AddDapr();
builder.Services.AddDaprClient();

var app = builder.Build();

app.UseAuthorization();
app.UseCloudEvents();
app.MapControllers();
app.MapSubscribeHandler();
app.Run();