using MessagePack;

namespace CatalogService.Contracts.FoodCategory.Responses;

[MessagePackObject]
public record FoodCategoryResponse
{ 
    [Key(0)]
    public string? Id { get; set; }
    [Key(1)]
    public required string Name { get; set; }
    [Key(2)]
    public bool Availability { get; set; } =  true;
};