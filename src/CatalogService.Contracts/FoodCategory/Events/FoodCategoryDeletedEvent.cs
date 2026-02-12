namespace CatalogService.Contracts.FoodCategory.Events;

public record FoodCategoryDeletedEvent
{
    public required string Id {get; init;}
    public DateTime DeletedOnUtc { get; init; }
}