using MessagePack;

namespace CatalogService.Contracts.Food.Requests;

[MessagePackObject]
public record FoodStockRequest
{
    [Key(0)]
    public required string FoodId { get; init; }
    [Key(1)]
    public int Quantity { get; init; }
}