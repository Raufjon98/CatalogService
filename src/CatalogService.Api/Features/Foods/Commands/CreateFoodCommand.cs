using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Food.Responses;
using MediatR;

namespace CatalogService.Api.Features.Foods.Commands;

public record CreateFoodCommand(CreateFoodRequest CreateFoodDto) : IRequest<FoodResponse>;

public class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand, FoodResponse>
{
    private readonly IFoodRepository _foodRepository;

    public CreateFoodCommandHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }
    public async Task<FoodResponse> Handle(CreateFoodCommand request, CancellationToken cancellationToken)
    {
        Food food = new Food()
        {
            Name = request.CreateFoodDto.Name,
            Stock = request.CreateFoodDto.Stock,
            FoodCategoryId = request.CreateFoodDto.FoodCategoryId,
            RestaurantId = request.CreateFoodDto.restaurantId,
            Price = request.CreateFoodDto.Price,
        };
        var result = await _foodRepository.CreateAsync(food, cancellationToken: cancellationToken);

        if (result is null)
        {
            throw new Exception("Couldn't create food");
        }
        
        FoodResponse createdFood = new FoodResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Stock = result.Stock,
            FoodCategoryId = result.FoodCategoryId,
            RestaurantId = result.RestaurantId,
            Price = result.Price,
            IsAvailable = result.Availability,
        };
        return createdFood;
    }
}