using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Events;
using CatalogService.Contracts.Food.Requests;
using CatalogService.Contracts.Food.Responses;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Foods.Commands;

public record CreateFoodCommand(CreateFoodRequest CreateFoodDto) : IRequest<FoodResponse>;

public class CreateFoodCommandHandler : IRequestHandler<CreateFoodCommand, FoodResponse>
{
    private readonly IFoodRepository _foodRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateFoodCommandHandler(IFoodRepository foodRepository, IPublishEndpoint publishEndpoint)
    {
        _foodRepository = foodRepository;
        _publishEndpoint = publishEndpoint;
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

        await _publishEndpoint.Publish(
            new FoodCreatedEvent
            {
                Id = result.Id,
                CreatedOnUtc = DateTime.UtcNow
            },
            cancellationToken);
        
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