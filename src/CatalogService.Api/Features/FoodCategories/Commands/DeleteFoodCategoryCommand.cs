using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.FoodCategories.Queries;
using MediatR;
using MongoDB.Bson.Serialization.Conventions;

namespace CatalogService.Api.Features.FoodCategories.Commands;

public record DeleteFoodCategoryCommand(string Id) : IRequest<bool>;

public class DeleteFoodCategoryCommandHandler : IRequestHandler<DeleteFoodCategoryCommand, bool>
{
    private readonly IFoodCategoryRepository _foodCategoryRepository;

    public DeleteFoodCategoryCommandHandler(IFoodCategoryRepository foodCategoryRepository)
    {
        _foodCategoryRepository = foodCategoryRepository;
    }
    
    public async Task<bool> Handle(DeleteFoodCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _foodCategoryRepository.DeleteAsync(request.Id, cancellationToken);
       
    }
}