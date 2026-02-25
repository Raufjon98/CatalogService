using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.FoodCategories.Queries;
using CatalogService.Contracts.FoodCategory.Events;
using MassTransit;
using MediatR;
using MongoDB.Bson.Serialization.Conventions;

namespace CatalogService.Api.Features.FoodCategories.Commands;

public record DeleteFoodCategoryCommand(string Id) : IRequest<bool>;

public class DeleteFoodCategoryCommandHandler : IRequestHandler<DeleteFoodCategoryCommand, bool>
{
    private readonly IFoodCategoryRepository _foodCategoryRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteFoodCategoryCommandHandler(IFoodCategoryRepository foodCategoryRepository, IPublishEndpoint publishEndpoint)
    {
        _foodCategoryRepository = foodCategoryRepository;
        _publishEndpoint = publishEndpoint;
    }
    
    public async Task<bool> Handle(DeleteFoodCategoryCommand request, CancellationToken cancellationToken)
    {
         await _foodCategoryRepository.DeleteAsync(request.Id, cancellationToken);
         await _publishEndpoint.Publish(
             new FoodCategoryDeletedEvent
             {
                 Id = request.Id,
                 DeletedOnUtc = DateTime.UtcNow
             },
             cancellationToken);

         return true;
    }
}