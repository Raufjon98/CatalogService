using MessagePack;

namespace CatalogService.Contracts.FoodCategory.Requests;

[MessagePackObject]
public record CreateFoodCategoryRequest
{
    [Key(0)]
    public required string Name { get; set; }
    [Key(1)]
    public bool IsAvailable { get; set; } =  true;
};