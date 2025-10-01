using MongoDB.Bson.Serialization.Attributes;

namespace RealEstateAPI.Domain.Entities;

[BsonCollection("owners")]
public class Owner : BaseEntity
{
    [BsonElement("idOwner")]
    public string IdOwner { get; set; } = string.Empty;
    
    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;
    
    [BsonElement("address")]
    public string Address { get; set; } = string.Empty;
    
    [BsonElement("photo")]
    public string Photo { get; set; } = string.Empty;
    
    [BsonElement("birthday")]
    public DateTime Birthday { get; set; }
    
    // Navigation property
    [BsonIgnore]
    public virtual ICollection<Property> Properties { get; set; } = new List<Property>();
}

// Atributo personalizado para definir el nombre de la colección
[AttributeUsage(AttributeTargets.Class)]
public class BsonCollectionAttribute : Attribute
{
    public string CollectionName { get; }

    public BsonCollectionAttribute(string collectionName)
    {
        CollectionName = collectionName;
    }
}