namespace RealEstateAPI.Application.DTOs;

public class OwnerDto
{
    public string Id { get; set; } = string.Empty;
    public string IdOwner { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Photo { get; set; } = string.Empty;
    public DateTime Birthday { get; set; }
}

public class PropertyDto
{
    public string Id { get; set; } = string.Empty;
    public string IdProperty { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string CodeInternal { get; set; } = string.Empty;
    public int Year { get; set; }
    public string IdOwner { get; set; } = string.Empty;
    public OwnerDto? Owner { get; set; }
    public string? MainImage { get; set; } // Solo una imagen principal
    public List<PropertyImageDto> Images { get; set; } = new();
    public List<PropertyTraceDto> Traces { get; set; } = new();
}

public class PropertyImageDto
{
    public string Id { get; set; } = string.Empty;
    public string IdPropertyImage { get; set; } = string.Empty;
    public string IdProperty { get; set; } = string.Empty;
    public string File { get; set; } = string.Empty;
    public bool Enabled { get; set; }
}

public class PropertyTraceDto
{
    public string Id { get; set; } = string.Empty;
    public string IdPropertyTrace { get; set; } = string.Empty;
    public DateTime DateSale { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public decimal Tax { get; set; }
    public string IdProperty { get; set; } = string.Empty;
}

// DTOs para responses de la API
public class PropertyListDto
{
    public string Id { get; set; } = string.Empty;
    public string IdProperty { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int Year { get; set; }
    public string OwnerName { get; set; } = string.Empty;
    public string MainImage { get; set; } = string.Empty;
}