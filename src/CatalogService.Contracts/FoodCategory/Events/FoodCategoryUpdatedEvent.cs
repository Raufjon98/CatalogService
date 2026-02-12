namespace CatalogService.Contracts.FoodCategory.Events;

public record FoodCategoryUpdatedEvent
{
    public required string Id {get; init;}
    public DateTime UpdatedOnUtc { get; init; }
}