using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Food.Responses;
using MediatR;

namespace CatalogService.Api.Features.Foods.Commands;

public record DecreaseFoodStockCommand(List<FoodStockRequest> Requests) : IRequest<List<FoodResponse>>;

public class DecreaseFoodStockCommandHandler : IRequestHandler<DecreaseFoodStockCommand, List<FoodResponse>>
{
    private readonly IFoodRepository _foodRepository;

    public DecreaseFoodStockCommandHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }

    public async Task<List<FoodResponse>> Handle(DecreaseFoodStockCommand command, CancellationToken cancellationToken)
    {
        var requestedFoods = command.Requests
            .ToDictionary(f => f.FoodId, f=>f.Quantity);
        string[] ids = requestedFoods.Keys.ToArray();
        var foods = await _foodRepository.GetAllAsync(ids, cancellationToken);
        var foodResponses = new List<FoodResponse>();
        foreach (var food in foods)
        {
            if (!requestedFoods.TryGetValue(food.Id, out var quantity))
                continue;
            
            if (food.Stock < quantity)
            {
                throw new Exception("Insufficient stock");
            }

            food.Stock -= quantity;
            var result = await _foodRepository.UpdateAsync(food, cancellationToken);
            
            FoodResponse updatedFood = new FoodResponse
            {
                Id = result.Id,
                Name = result.Name,
                Stock = result.Stock,
                FoodCategoryId = result.FoodCategoryId,
                RestaurantId = result.RestaurantId,
                Price = result.Price,
                IsAvailable = result.Availability,
            };
            foodResponses.Add(updatedFood);
        }
        return foodResponses;
    }
}