using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using CatalogService.Contracts.Food.Events;
using MassTransit;
using MediatR;

namespace CatalogService.Api.Features.Foods.Commands;

public record DeleteFoodCommand(string FoodId) :IRequest<bool>;

public class DeleteFoodCommandHandler : IRequestHandler<DeleteFoodCommand, bool>
{
    private readonly IFoodRepository _foodRepository;
    private readonly IPublishEndpoint _publishEndpoint;

    public DeleteFoodCommandHandler(IFoodRepository foodRepository, IPublishEndpoint publishEndpoint)
    {
        _foodRepository = foodRepository;
        _publishEndpoint = publishEndpoint;
    }
    public async Task<bool> Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
    {
        var food = await _foodRepository.GetAsync(request.FoodId, cancellationToken: cancellationToken);
        
        if (food is null)
        {
            throw new NotFoundException(nameof(Food), request.FoodId);
        }
        
        await _foodRepository.DeleteAsync(food, cancellationToken);
        
        await _publishEndpoint.Publish(
            new FoodDeletedEvent
            {
                Id = food.Id,
                DeletedOnUtc = DateTime.UtcNow
            },
            cancellationToken);
        
        return true;
    }
}