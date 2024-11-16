namespace TechnicoRMP.Shared.Dtos;

public sealed class GetUsersInfoDto : IDto
{
    public long Id { get; set; }
    public required string Name { get; set; }
    public required string VatNumber { get; set; }
    public required string Surname { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public required string Email { get; set; }
    public bool IsActive { get; set; } = true;
    public List<PropertyItemsDto> PropertyItems { get; set; } = [];
    public List<PropertyRepairResponseDTO> Repairs { get; set; } = [];
}

