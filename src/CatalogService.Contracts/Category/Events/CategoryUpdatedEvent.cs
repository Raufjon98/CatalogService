namespace CatalogService.Contracts.Category.Events;

public record CategoryUpdatedEvent
{
    public required string Id { get; init; }
    public DateTime UpdatedOnUtc { get; init; }
}