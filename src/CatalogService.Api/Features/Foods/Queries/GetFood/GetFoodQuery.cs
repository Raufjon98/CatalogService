using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Responses;
using MediatR;

namespace CatalogService.Api.Features.Foods.Queries;

public record GetFoodQuery(string FoodId) : IRequest<FoodResponse>;

public class GetFoodQueryHandler : IRequestHandler<GetFoodQuery, FoodResponse>
{
    private readonly IFoodRepository _foodRepository;
    public GetFoodQueryHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }
    public async Task<FoodResponse> Handle(GetFoodQuery request, CancellationToken cancellationToken)
    {
        var food = await _foodRepository.GetAsync(request.FoodId, cancellationToken);

        if (food is null)
        {
            throw new NotFoundException(nameof(Food), request.FoodId);
        }
        var foodResponse = new FoodResponse()
        {
            Id = food.Id,
            Name = food.Name,
            Stock = food.Stock,
            FoodCategoryId = food.FoodCategoryId,
            RestaurantId = food.RestaurantId,
            Price = food.Price,
            IsAvailable = food.Availability,
        };
        return foodResponse;
    }
}