using System;
using System.IO;
using MassTransit;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StorageService.Api.Domain.Visitors;
using StorageService.Api.Infrastructure;
using StorageService.Api.MessageApi;
using StorageService.Api.MessageApi.Consumers;
using StorageService.Api.MessageApi.Mappers;
using StorageService.Api.MessageApi.Settings;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddOptions<AppSettings>()
    .Configure<IConfiguration>((settings, config) => config.Bind("AppSettings", settings));

builder.Services.AddScoped<IVisitorRepository, VisitorFileRepository>();

builder.Services.AddAutoMapper(typeof(MessagingProfile));
builder.Services.AddMassTransit<IBus>(options =>
{
    options.SetKebabCaseEndpointNameFormatter();
    
    options.AddConsumer<CreateVisitorConsumer>();

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

app.Run();
