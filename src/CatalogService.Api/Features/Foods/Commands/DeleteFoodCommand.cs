using CatalogService.Api.Domain.Entities;
using CatalogService.Api.Features.Common.Exceptions;
using CatalogService.Api.Features.Common.interfaces;
using MediatR;

namespace CatalogService.Api.Features.Foods.Commands;

public record DeleteFoodCommand(string FoodId) :IRequest<bool>;

public class DeleteFoodCommandHandler : IRequestHandler<DeleteFoodCommand, bool>
{
    private readonly IFoodRepository _foodRepository;

    public DeleteFoodCommandHandler(IFoodRepository foodRepository)
    {
        _foodRepository = foodRepository;
    }
    public async Task<bool> Handle(DeleteFoodCommand request, CancellationToken cancellationToken)
    {
        var food = await _foodRepository.GetAsync(request.FoodId, cancellationToken: cancellationToken);
        if (food is null)
        {
            throw new NotFoundException(nameof(Food), request.FoodId);
        }
        return  await _foodRepository.DeleteAsync(food, cancellationToken);
    }
}