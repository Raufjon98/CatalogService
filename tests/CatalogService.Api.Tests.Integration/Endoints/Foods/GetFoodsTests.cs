using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Interfaces;
using FluentAssertions;
using MagicOnion.Client;
using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

namespace CatalogService.Api.Tests.Integration.Endoints.Foods;

public class GetFoodsTests : IClassFixture<CatalogServiceApiFactory>
{
    private readonly CatalogServiceApiFactory _factory;
    private readonly IFoodService _foodService;

    public GetFoodsTests(CatalogServiceApiFactory factory)
    {
        _factory = factory;
        var channel = _factory.CreateGrpcChannel();
        _foodService = MagicOnionClient.Create<IFoodService>(channel);
    }

    [Fact]
    public async Task Should_ReturnAllFoods_WhenFoodsExists()
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
        var response = await _foodService.GetAllFoodsAsync();

        //Assert
        response.Should().NotBeNull();
        response.Should().ContainEquivalentOf(createFoodResponse);
    }

    [Fact]
    public async Task Should_ReturnEmptyFoodResponseList_WhenFoodsDoesNotExist()
    {
        //Act
        var response = await _foodService.GetAllFoodsAsync();

        //Assert
        response.Should().NotBeNullOrEmpty();
    }
}