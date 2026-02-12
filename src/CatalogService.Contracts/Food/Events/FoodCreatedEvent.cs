namespace CatalogService.Contracts.Food.Events;

public record FoodCreatedEvent
{
    public required string Id {get; init;}
    public DateTime CreatedOnUtc { get; init; }
}