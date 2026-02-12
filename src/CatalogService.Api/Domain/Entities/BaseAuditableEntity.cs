using System.Runtime.InteropServices.JavaScript;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CatalogService.Api.Domain.Entities;

public abstract class BaseAuditableEntity
{
    [BsonId]
    [BsonElement("_id"), BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [BsonElement("created_at"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime CreatedAt { get; set; }
    [BsonElement("modified_at"), BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
    public DateTime ModifiedAt { get; set; }
    [BsonElement("isDeleted"), BsonRepresentation(BsonType.Boolean)]
    public bool IsDeleted { get; set; }
    
    protected BaseAuditableEntity()
    {
        Id = ObjectId.GenerateNewId().ToString();
        CreatedAt = DateTime.UtcNow;
        ModifiedAt = DateTime.UtcNow;
        IsDeleted = false;
    }
}