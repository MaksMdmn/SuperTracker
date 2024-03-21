using System;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PixelService.Api.Application;
using PixelService.Api.Application.Interfaces;
using PixelService.Api.Infrastructure.Blob;
using PixelService.Api.Infrastructure.Interfaces;
using PixelService.Api.WebApi.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions<AppSettings>()
    .Configure<IConfiguration>((settings, config) => config.Bind("AppSettings", settings));

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<IDataCollector, HttpDataCollector>();
builder.Services.AddScoped<IBlobContentProvider, MemoryBlobContentProvider>();

builder.Services.AddMassTransit<IBus>(options =>
{
    options.SetKebabCaseEndpointNameFormatter();
    
    options.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(builder.Configuration["AppSettings:RabbitMq:ConnectionString"]));

        configurator.ConfigureEndpoints(context);
    });
});
    
var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();
app.UseHttpsRedirection();

app.MapGet("/track", (IDataCollector dataCollector, IBlobContentProvider blobs) =>
{
    dataCollector.CollectAsync();

    var image = blobs.Download( "some_url");
    
    return Results.File(image, "image/gif");
});

app.Run();