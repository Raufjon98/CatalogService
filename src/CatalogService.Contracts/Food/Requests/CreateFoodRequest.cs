using MessagePack;

namespace CatalogService.Contracts.Food.Requests;

[MessagePackObject]
public record CreateFoodRequest
{
    [Key(0)]
    public required string Name { get; set; }
    [Key(1)]
    public required string FoodCategoryId { get; set; }
    [Key(2)]
    public required string restaurantId { get; set; }
    [Key(3)]
    public decimal Price { get; set; }
    [Key(4)]
    public int Stock { get; set; }
};