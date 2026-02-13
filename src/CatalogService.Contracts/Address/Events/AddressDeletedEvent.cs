namespace CatalogService.Contracts.Address.Events;

public record AddressDeletedEvent
{
    public required string Id { get; init; }
    public DateTime DeletedOnUtc { get; init; }
}