using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Api.Domain.Entities;

public class Food : BaseAuditableEntity
{
    [BsonElement("name"), BsonRepresentation(BsonType.String)]
    public required string Name { get; set; }

    [BsonElement("stock"), BsonRepresentation(BsonType.Int64)]
    public long Stock { get; set; } = 0;
    [BsonElement("FoodCategoryId"), BsonRepresentation(BsonType.String)]
    public required string FoodCategoryId { get; set; }
    [BsonElement("restaurantId"), BsonRepresentation(BsonType.String)]
    public required string RestaurantId { get; set; }
    [BsonElement("price"), BsonRepresentation(BsonType.Double)]
    public required decimal Price { get; set; }

    [BsonElement("availability"), BsonRepresentation(BsonType.Boolean)]
    public bool Availability { get; set; } = true;
}