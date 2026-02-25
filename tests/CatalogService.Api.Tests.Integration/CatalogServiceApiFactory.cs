using CatalogService.Api.Features.Common.Behaviour;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Features.Data;
using CatalogService.Api.Features.Foods.Queries;
using CatalogService.Api.Infrastructure.Repositories;
using FluentValidation;
using Grpc.Net.Client;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using StackExchange.Redis;
using Testcontainers.MongoDb;
using Testcontainers.RabbitMq;
using Testcontainers.Redis;

namespace CatalogService.Api.Tests.Integration;

public class CatalogServiceApiFactory : WebApplicationFactory<IApiMarker>, IAsyncLifetime
{
    private readonly MongoDbContainer _mongoDbContainer =
        new MongoDbBuilder("mongo:7.0")
            .WithCleanUp(true)
            .Build();

    private readonly RabbitMqContainer _rabbitMqContainer =
        new RabbitMqBuilder("rabbitmq:management")
            .WithCleanUp(true)
            .Build();

    private readonly RedisContainer _redisContainer =
        new RedisBuilder("redis")
            .WithCleanUp(true)
            .Build();

    private GrpcChannel? _channel;

    public GrpcChannel CreateGrpcChannel()
    {
        if (_channel == null)
        {
            var httpClient = CreateClient();
            _channel = GrpcChannel.ForAddress(httpClient.BaseAddress!, new GrpcChannelOptions
            {
                HttpClient = httpClient
            });
        }

        return _channel;
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging(logging => logging.ClearProviders());
        builder.UseEnvironment("IntegrationTest");

        builder.ConfigureAppConfiguration((ctx, config) =>
        {
            config.AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["ConnectionStrings:MongoDb"] = _mongoDbContainer.GetConnectionString(),
             });
        });

        builder.ConfigureTestServices(services =>
        {
            
            services.RemoveAll<IMongoClient>();
            services.RemoveAll<MongoDbService>();
            services.AddSingleton<IMongoClient>(_ => new MongoClient(_mongoDbContainer.GetConnectionString()));
            services.AddSingleton<MongoDbService>();

            services.AddScoped<IFoodRepository, FoodRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IFoodCategoryRepository, FoodCategoryRepository>();
            services.AddScoped<ICuisineRepository, CuisineRepository>();
            services.AddScoped<IAddressRepository, AddressRepository>();
            services.AddScoped<IRestaurantRepository, RestaurantRepository>();

            services.AddMediatR(typeof(GetFoodsQueryHandler).Assembly); 
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddValidatorsFromAssembly(typeof(GetFoodsQueryHandler).Assembly);
            
            services.AddMassTransitTestHarness(x =>
            {
                x.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host(_rabbitMqContainer.GetConnectionString());
                    cfg.ConfigureEndpoints(ctx);
                });
            });

            services.RemoveAll<IDistributedCache>();
            services.RemoveAll<IDatabase>();
            services.RemoveAll<IConnectionMultiplexer>();
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = _redisContainer.GetConnectionString();
            });
        });
    }

    public async Task InitializeAsync()
    {
        await Task.WhenAll(
            _mongoDbContainer.StartAsync(),
            _rabbitMqContainer.StartAsync(),
            _redisContainer.StartAsync());
        Console.WriteLine($"MonGo connection is:{_mongoDbContainer.GetConnectionString()}");
    }

    public new async Task DisposeAsync()
    {
        await _mongoDbContainer.StopAsync();
        await _rabbitMqContainer.StopAsync();
        await _redisContainer.StopAsync();
    }
}