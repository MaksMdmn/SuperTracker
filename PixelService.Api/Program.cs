using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using PixelService.Api.Application.Interfaces;
using PixelService.Api.Infrastructure.Extensions;
using PixelService.Api.Infrastructure.Interfaces;
using PixelService.Api.WebApi.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSettings();
builder.Services.AddMassTransitServices(builder.Configuration["AppSettings:RabbitMq:ConnectionString"]);

builder.Services.AddServices();
    
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/track", (IUserDataCollector dataCollector, IBlobContentProvider blobs) =>
{
    dataCollector.CollectAsync();

    var image = blobs.Download( "some_url");
    
    return Results.File(image, "image/gif");
});

app.Run();

namespace PixelService.Api
{
}