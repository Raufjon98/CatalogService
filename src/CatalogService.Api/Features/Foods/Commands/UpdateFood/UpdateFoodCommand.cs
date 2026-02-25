using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Events;
using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Food.Responses;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Foods.Commands;

public record UpdateFoodCommand(string FoodId, CreateFoodRequest CreateFoodDto) : IRequest<FoodResponse>;
public class UpdatefoodCommandHandler : IRequestHandler<UpdateFoodCommand, FoodResponse>
{
    private readonly IFoodRepository _foodRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public UpdatefoodCommandHandler(IFoodRepository foodRepository, IPublishEndpoint publishEndpoint)
    {
        _foodRepository = foodRepository;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<FoodResponse> Handle(UpdateFoodCommand request, CancellationToken cancellationToken)
    {
        var food = await _foodRepository.GetAsync(request.FoodId, cancellationToken);
        
        if (food == null)
        {
            throw new NotFoundException(nameof(Food), request.FoodId);
        }
        
        food.Name = request.CreateFoodDto.Name!;
        food.Stock = request.CreateFoodDto.Stock;
        food.FoodCategoryId = request.CreateFoodDto.FoodCategoryId;
        food.RestaurantId = request.CreateFoodDto.restaurantId;
        food.Price = request.CreateFoodDto.Price;
        food.ModifiedAt = DateTime.UtcNow;
        var result = await _foodRepository.UpdateAsync(food, cancellationToken);
        
        if (result is null)
        {
            throw new Exception("Could not update food");
        }

        await _publishEndpoint.Publish(
            new FoodUpdatedEvent
            {
                Id = result.Id,
                UpdatedOnUtc = DateTime.UtcNow
            },
            cancellationToken);
        
        FoodResponse updatedFood = new FoodResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Stock = result.Stock,
            FoodCategoryId = result.FoodCategoryId,
            RestaurantId = result.RestaurantId,
            Price = result.Price,
            IsAvailable = result.Availability,
        };
        return updatedFood;
    }
}
