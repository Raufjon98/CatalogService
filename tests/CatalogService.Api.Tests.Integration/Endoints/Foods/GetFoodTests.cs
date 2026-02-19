using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Interfaces;
using FluentAssertions;
using Grpc.Core;
using MagicOnion.Client;
using MongoDB.Bson;

namespace CatalogService.Api.Tests.Integration.Endoints.Foods;

public class GetFoodTests : IClassFixture<CatalogServiceApiFactory>
{
    private readonly CatalogServiceApiFactory _factory;
    private readonly IFoodService _foodService;

    public GetFoodTests(CatalogServiceApiFactory factory)
    {
        _factory = factory;
        var channel = _factory.CreateGrpcChannel();
        _foodService = MagicOnionClient.Create<IFoodService>(channel);
    }

    [Fact]
    public async Task GetFood_ReturnsFoodResponse_WhenFoodExists()
    {
        //Arrange
        var createFoodRequest = new CreateFoodRequest
        {
            Name = "Test Food",
            restaurantId = ObjectId.GenerateNewId().ToString(),
            FoodCategoryId = ObjectId.GenerateNewId().ToString(),
            Price = 10,
            Stock = 55
        };
        var createFoodResponse = await _foodService.CreateFoodAsync(createFoodRequest);
        createFoodResponse.Should().NotBeNull();
        createFoodResponse.Id.Should().NotBeEmpty();
        var foodId = createFoodResponse.Id;
        
        //Act
        var response = await _foodService.GetFoodAsync(foodId);
        
        //Assert
        response.Should().NotBeNull();
        response.Should().BeEquivalentTo(createFoodResponse);
    }

    [Fact]
    public async Task GetFood_ThrowsException_WhenFoodDoesNotExist()
    {
        //Arrange
        var foodId = ObjectId.GenerateNewId().ToString();
        
        //Act
        Func<Task> act = async () => await _foodService.GetFoodAsync(foodId);
        
        //Assert
        await act.Should().ThrowAsync<RpcException>($"Entity Food with key {foodId} not found!");

    }
}