using System.Text.Json.Serialization;
using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

public class PropertyRepairResponseDTO : IDto
{
    public long Id { get; set; }
    public DateTime Date { get; set; }

    [JsonPropertyName("formatted_date")]
    public string FormattedDate => this.Date.ToString("yyyy-MM-dd");
    public EnTypeOfRepair TypeOfRepair { get; set; }
    public required string Address { get; set; }
    public EnRepairStatus RepairStatus { get; set; }
    public double Cost { get; set; }
    public long UserId { get; set; }
    public bool IsActive { get; set; }
    public string? Description { get; set; }

}
