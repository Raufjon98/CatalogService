using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Food.Responses;
using CatalogService.Contracts.Interfaces;
using FluentAssertions;
using Grpc.Core;
using MagicOnion.Client;

namespace CatalogService.Api.Tests.Integration.Endoints.Foods;

public class CreateFoodTests : IClassFixture<CatalogServiceApiFactory>
{
    private readonly CatalogServiceApiFactory _factory;
    private readonly IFoodService _foodService;
    
    public CreateFoodTests(CatalogServiceApiFactory factory)
    {
        _factory = factory;
        var channel = _factory.CreateGrpcChannel();
        _foodService = MagicOnionClient.Create<IFoodService>(channel);
    }

    [Fact]
    public async Task CreateFood_CreatesFood_WhenDataIsValid()
    {
        //Arrange
        var createFoodRequest = new CreateFoodRequest
        {
            Name = "Steamed salmon",
            FoodCategoryId = Guid.NewGuid().ToString(),
            restaurantId = Guid.NewGuid().ToString(),
            Stock = 50,
            Price = 16
        };
        
        //Act
        var response  = await _foodService.CreateFoodAsync(createFoodRequest);
        
        //Assert
        response.Should().NotBeNull();
        response.Should().BeOfType<FoodResponse>();
        response.Name.Should().BeEquivalentTo(createFoodRequest.Name);
    }

    [Fact]
    public async Task CreateFood_ThrowsException_WhenDataIsInvalid()
    {
        // Arrange
        var createFoodRequest = new CreateFoodRequest
        {
            Name = null,
            FoodCategoryId = Guid.Empty.ToString(),
            restaurantId = Guid.Empty.ToString(),
            Stock = 50,
        };
    
        // Act
        Func<Task> act = async () => await _foodService.CreateFoodAsync(createFoodRequest);
    
        // Assert
        await act.Should().ThrowExactlyAsync<RpcException>();
    }
}