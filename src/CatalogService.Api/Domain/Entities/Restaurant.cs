using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Api.Domain.Entities;

public class Restaurant : BaseAuditableEntity
{
    [BsonElement("name"), BsonRepresentation(BsonType.String)]
    public required string Name { get; set; }
    [BsonElement("description"), BsonRepresentation(BsonType.String)]
    public string? Description { get; set; }
    [BsonElement("categoryId"), BsonRepresentation(BsonType.String)]
    public required string CategoryId  {get; set;} 
    [BsonElement("cuisineId"), BsonRepresentation(BsonType.String)]
    public string? CuisineId { get; set; } 
    [BsonElement("isActive"), BsonRepresentation(BsonType.Boolean)]    
    public bool IsActive { get; set; } = true;
    [BsonElement("addressId"), BsonRepresentation(BsonType.String)]
    public required string AddressId { get; set; }
    [BsonElement("phone"), BsonRepresentation(BsonType.String)]
    public string? Phone { get; set; }
    [BsonElement("email"), BsonRepresentation(BsonType.String)]
    public string? Email { get; set; }
    [BsonElement("availability"), BsonRepresentation(BsonType.Boolean)]
    public bool Availability { get; set; } =  true;
}