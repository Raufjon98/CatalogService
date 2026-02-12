using MessagePack;

namespace CatalogService.Contracts.Food.Responses;

[MessagePackObject]
public record FoodResponse
{
    [Key(0)]
    public string? Id { get; set; }
    [Key(1)]
    public required string Name { get; set; }
    [Key(2)]
    public long Stock { get; set; }
    [Key(3)]
    public required string FoodCategoryId { get; set; }
    [Key(4)]
    public required string RestaurantId { get; set; }
    [Key(5)]
    public decimal Price { get; set; }
    [Key(6)]
    public bool IsAvailable { get; set; }
};