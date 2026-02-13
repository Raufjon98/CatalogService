namespace CatalogService.Contracts.Food.Events;

public record FoodDeletedEvent
{
    public required string Id {get; init;}
    public DateTime DeletedOnUtc { get; init; }
}