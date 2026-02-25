using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.FoodCategory.Responses;
using MediatR;

namespace CatalogService.Api.Features.FoodCategories.Queries;

public record GetFoodCategoryQuery(string Id) : IRequest<FoodCategoryResponse>;
public class GetFoodCategoryQueryHandler : IRequestHandler<GetFoodCategoryQuery, FoodCategoryResponse>
{
    private readonly IFoodCategoryRepository _foodCategoryRepository;

    public GetFoodCategoryQueryHandler(IFoodCategoryRepository foodCategoryRepository)
    {
        _foodCategoryRepository = foodCategoryRepository;
    }
    public async Task<FoodCategoryResponse> Handle(GetFoodCategoryQuery request, CancellationToken cancellationToken)
    {
        var foodCategory = await _foodCategoryRepository.GetAsync(request.Id, cancellationToken);

        if (foodCategory == null)
        {
            throw new NotFoundException(nameof(FoodCategory), request.Id);
        }
        
        FoodCategoryResponse foodCategoryResponse = new FoodCategoryResponse()
        {
            Id = foodCategory.Id,
            Name = foodCategory.Name,
            Availability = foodCategory.Availability,
        };
        return foodCategoryResponse;
    }
}