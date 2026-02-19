using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Interfaces;
using FluentAssertions;
using Grpc.Core;
using MagicOnion.Client;
using MongoDB.Bson;

namespace CatalogService.Api.Tests.Integration.Endoints.Foods;

public class DeleteFoodTests : IClassFixture<CatalogServiceApiFactory>
{
    private readonly CatalogServiceApiFactory _factory;
    private readonly IFoodService _foodService;

    public DeleteFoodTests(CatalogServiceApiFactory factory)
    {
        _factory = factory;
        var channel = _factory.CreateGrpcChannel();
        _foodService = MagicOnionClient.Create<IFoodService>(channel);
    }

    [Fact]
    public async Task DeleteFood_DeletesFood_WhenFoodExists()
    {
        //Arrange
        var createFoodRequest = new CreateFoodRequest
        {
            Name = "Pizza",
            Price = 57,
            FoodCategoryId = ObjectId.GenerateNewId().ToString(),
            restaurantId = ObjectId.GenerateNewId().ToString(),
            Stock = 15
        };
        var createFoodResponse = await _foodService.CreateFoodAsync(createFoodRequest);
        createFoodResponse.Should().NotBeNull();
        createFoodResponse.Name.Should().Be("Pizza");

        //Act
        var response = await _foodService.DeleteFoodAsync(createFoodResponse.Id!);

        //Assert
        response.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteFood_ThrowsException_WhenFoodDoesNotExist()
    {
        //Arrange
        var foodId = ObjectId.GenerateNewId().ToString();
        
        //Act
        Func<Task> act = async() => await _foodService.DeleteFoodAsync(foodId);
        
        //
        act.Should().ThrowAsync<RpcException>($"Entity Food with key {foodId} not found!");
    }
}