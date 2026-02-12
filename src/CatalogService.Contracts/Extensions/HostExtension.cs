using CatalogService.Contracts.Interfaces;
using Grpc.Core;
using Grpc.Net.Client;
using MagicOnion.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CatalogService.Contracts.Extensions;

public static class HostExtension
{
    public static IServiceCollection AddCatalogServiceContracts(this IServiceCollection services)
    {
        var catalogServiceUrl = "https://localhost:5001";
       
        services.AddSingleton<IAddressService>(_ =>
            MagicOnionClient.Create<IAddressService>(GrpcChannel.ForAddress(catalogServiceUrl)));
        services.AddSingleton<ICategoryService>(_ =>
            MagicOnionClient.Create<ICategoryService>(GrpcChannel.ForAddress(catalogServiceUrl)));
        services.AddSingleton<IFoodService>(_ =>
            MagicOnionClient.Create<IFoodService>(GrpcChannel.ForAddress(catalogServiceUrl)));
        services.AddSingleton<IFoodCategoryService>( _ =>
            MagicOnionClient.Create<IFoodCategoryService>(GrpcChannel.ForAddress(catalogServiceUrl)));
        services.AddSingleton<IRestaurantService>(_ =>
            MagicOnionClient.Create<IRestaurantService>(GrpcChannel.ForAddress(catalogServiceUrl)));
        services.AddSingleton<ICuisineService>(_ =>
            MagicOnionClient.Create<ICuisineService>(GrpcChannel.ForAddress(catalogServiceUrl)));
        
        return services;
    }
}