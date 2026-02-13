namespace CatalogService.Contracts.Category.Events;

public record CategoryDeletedEvent
{
    public required string Id { get; init; }
    public DateTime DeletedOnUtc { get; init; }
}