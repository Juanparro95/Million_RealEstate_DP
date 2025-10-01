using MongoDB.Bson.Serialization.Attributes;

namespace RealEstateAPI.Domain.Entities;

[BsonCollection("propertyImages")]
public class PropertyImage : BaseEntity
{
    [BsonElement("idPropertyImage")]
    public string IdPropertyImage { get; set; } = string.Empty;
    
    [BsonElement("idProperty")]
    public string IdProperty { get; set; } = string.Empty;
    
    [BsonElement("file")]
    public string File { get; set; } = string.Empty;
    
    [BsonElement("enabled")]
    public bool Enabled { get; set; } = true;
    
    // Navigation property
    [BsonIgnore]
    public virtual Property Property { get; set; } = null!;
}