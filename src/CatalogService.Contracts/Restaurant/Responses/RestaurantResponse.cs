using MessagePack;

namespace CatalogService.Contracts.Restaurant.Responses;

[MessagePackObject]
public record RestaurantResponse
{
    [Key(0)]
    public string? Id { get; set; }
    [Key(1)]
    public required string Name { get; set; }
    [Key(2)]
    public string? Description { get; set; }
    [Key(3)]
    public required string CategoryId  {get; set;}
    [Key(4)]
    public string? CuisineId { get; set; }
    [Key(5)]
    public required string AddressId { get; set; }
    [Key(6)]
    public string? Phone { get; set; }
    [Key(7)]
    public string? Email { get; set; }
    [Key(8)]
    public bool IsAvailable { get; set; }
};