namespace CatalogService.Contracts.Food.Events;

public record FoodUpdatedEvent
{
    public required string Id {get; init;}
    public DateTime UpdatedOnUtc { get; init; }
}