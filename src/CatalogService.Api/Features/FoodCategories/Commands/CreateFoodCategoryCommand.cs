using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.FoodCategory.Events;
using CatalogService.Contracts.FoodCategory.Requests;
using CatalogService.Contracts.FoodCategory.Responses;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CatalogService.Api.Features.FoodCategories.Commands;

public record CreateFoodCategoryCommand(CreateFoodCategoryRequest FoodCategory) : IRequest<FoodCategoryResponse>;

public class CreateFoodCategoryCommandHandler : IRequestHandler<CreateFoodCategoryCommand, FoodCategoryResponse>
{
    private readonly IFoodCategoryRepository _foodCategoryRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateFoodCategoryCommandHandler(IFoodCategoryRepository foodCategoryRepository, IPublishEndpoint publishEndpoint)
    {
        _foodCategoryRepository = foodCategoryRepository;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<FoodCategoryResponse> Handle(CreateFoodCategoryCommand request, CancellationToken cancellationToken)
    {
        FoodCategory foodCategory = new FoodCategory()
        {
            Name = request.FoodCategory.Name,
            Availability = request.FoodCategory.IsAvailable,
        };
        var result = await _foodCategoryRepository.CreateAsync(foodCategory, cancellationToken);

        if (result is null)
        {
            throw new Exception("Couldn't create food category");
        }

        await _publishEndpoint.Publish(
            new FoodCategoryCreatedEvent
            {
                Id = foodCategory.Id,
                CreatedOnUtc = DateTime.UtcNow
            },
            cancellationToken);
        
        FoodCategoryResponse foodCategoryResponse = new FoodCategoryResponse()
        {
            Id = result.Id,
            Name = result.Name,
            Availability = result.Availability,
        };
        return foodCategoryResponse;
    }
}