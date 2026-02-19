using CatalogService.Api.Extensions;
using CatalogService.Api.Features.Common.Behaviour;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Features.Data;
using CatalogService.Api.Features.Foods.Queries;
using CatalogService.Api.Infrastructure.Interceptors;
using CatalogService.Api.Infrastructure.Repositories;
using FluentValidation;
using MassTransit;
using Scalar.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using MongoDB.Driver;
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

if (!builder.Environment.IsEnvironment("IntegrationTest"))
{
    builder.Services.AddMassTransit(configuration =>
    {
        configuration.UsingRabbitMq((ctx, cfg) =>
        {
            cfg.Host(rabbitConnectionString);
            cfg.ExchangeType = ExchangeType.Fanout;
            cfg.ConfigureEndpoints(ctx);
        });
    });
}

builder.Services.AddOpenApi();

builder.Services.AddSingleton<IMongoClient>(_ =>
{
    var connectionString = builder.Configuration.GetConnectionString("MongoDb")
                           ?? throw new InvalidOperationException("MongoDB connection string 'MongoDb' is not configured.");
    return new MongoClient(connectionString);
});

builder.Services.AddSingleton<MongoDbService>();
builder.Services.AddScoped<IFoodRepository, FoodRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();
builder.Services.AddScoped<ICuisineRepository, CuisineRepository>();
builder.Services.AddScoped<IAddressRepository, AddressRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();
builder.Services.AddMediatR(typeof(IApiMarker).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddValidatorsFromAssembly(typeof(IApiMarker).Assembly);

builder.Services.AddGrpc(options => { options.Interceptors.Add<ExceptionInterceptor>(); });
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