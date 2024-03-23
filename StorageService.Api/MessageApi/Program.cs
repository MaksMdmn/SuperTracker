using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StorageService.Api.Infrastructure.Extensions;
using StorageService.Api.MessageApi.Extensions;

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

app.Run();