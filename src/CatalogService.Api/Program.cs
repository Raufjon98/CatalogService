using System.Reflection;
using CatalogService.Api.Extensions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Features.Data;
using CatalogService.Api.Infrastructure.Interceptors;
using CatalogService.Api.Infrastructure.Repositories;
using MassTransit;
using Scalar.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.ConfigureKestrel(options =>
{
    options.ListenAnyIP(5001, listenOptions =>
    {
        listenOptions.UseHttps();              
        listenOptions.Protocols = HttpProtocols.Http2;
    });
});
var rabbitConnectionString = builder.Configuration["MessageBroker:Host"];

builder.Services.AddMassTransit(configuration =>
{
    configuration.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(rabbitConnectionString);
        cfg.ExchangeType = ExchangeType.Fanout;
        cfg.ConfigureEndpoints(ctx);
    });
});

builder.Services.AddOpenApi();
builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IFoodCategoryRepository,FoodCategoryRepository>();
builder.Services.AddScoped<ICuisineRepository, CuisineRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<ExceptionInterceptor>();
});
builder.Services.AddMagicOnion();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.MapEndpoints();

app.MapGet("/mongo/ping", (MongoDbService mongoDbService) =>
{
    var dbName = mongoDbService.Database?.DatabaseNamespace.DatabaseName;
    return Results.Ok(new
    {
        Status = "Connected",
        Database = dbName
    });
});
app.MapMagicOnionService();

app.Run();

