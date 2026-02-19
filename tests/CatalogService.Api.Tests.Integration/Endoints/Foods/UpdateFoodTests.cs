using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Interfaces;
using FluentAssertions;
using Grpc.Core;
using MagicOnion.Client;
using MongoDB.Bson;

namespace CatalogService.Api.Tests.Integration.Endoints.Foods;

public class UpdateFoodTests : IClassFixture<CatalogServiceApiFactory>
{
    private readonly CatalogServiceApiFactory _factory;
    private readonly IFoodService _foodService;

    public UpdateFoodTests(CatalogServiceApiFactory factory)
    {
        _factory = factory;
        var channel = _factory.CreateGrpcChannel();
        _foodService = MagicOnionClient.Create<IFoodService>(channel);
    }

    [Fact]
    public async Task UpdateFood_UpdatesFood_WhenDataIsValid()
    {
        //Arrange
        var createFoodRequest = new CreateFoodRequest
        {
            Name = "soup",
            Price = 15,
            FoodCategoryId = Guid.Empty.ToString(),
            restaurantId = Guid.Empty.ToString(),
            Stock = 50,
        };
        
        var createFoodResponse = await _foodService.CreateFoodAsync(createFoodRequest);
        createFoodResponse.Id.Should().NotBeNull();
        createFoodResponse.Name.Should().Be("soup");
        var foodId = createFoodResponse.Id;
        
        var updateFoodRequest = new CreateFoodRequest
        {
            Name = "Rice soup",
            FoodCategoryId = Guid.Empty.ToString(),
            restaurantId = Guid.Empty.ToString(),
            Price = 14,
            Stock = 50,
        };
        
        //Act
        var response = await _foodService.UpdateFoodAsync(foodId!, updateFoodRequest);
        
        //Assert
        response.Should().NotBeNull();
        response.Name.Should().Be("Rice soup");
        response.FoodCategoryId.Should().Be(createFoodResponse.FoodCategoryId); 
        response.Id.Should().Be(foodId!);
    }

    [Fact]
    public async Task UpdateFood_ThrowsException_WhenFoodNotFound()
    {
        //Arrange
        var foodId = ObjectId.GenerateNewId().ToString();
        var updateFoodRequest = new CreateFoodRequest
        {
            Name = "Rice soup",
            FoodCategoryId = Guid.Empty.ToString(),
            restaurantId = Guid.Empty.ToString(),
            Price = 14,
            Stock = 50,
        };
        
        //Act
        Func<Task> act = async() =>  await _foodService.UpdateFoodAsync(foodId, updateFoodRequest);
        
        //Assert
        await act.Should().ThrowExactlyAsync<RpcException>($"Entity Food with key {foodId} not found!");
    }
}