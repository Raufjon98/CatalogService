using MessagePack;

namespace CatalogService.Contracts.Restaurant.Requests;

[MessagePackObject]
public record CreateRestaurantRequest
{
    [Key(0)]
    public required string Name { get; set; }
    [Key(1)]
    public string? Description { get; set; }
    [Key(2)]
    public required string CategoryId  {get; set;}
    [Key(3)]
    public string? CuisineId { get; set; }
    [Key(4)]
    public required string AddressId { get; set; }
    [Key(5)]
    public string? Phone { get; set; }
    [Key(6)]
    public string? Email { get; set; }
};