using TechnicoRMP.Models;

namespace TechnicoMVCApp.Models
{
    public class PropertyItemViewModel
    {
        public long Id { get; set; }
        public required string E9Number { get; set; }
        public required string Address { get; set; }
        public required int YearOfConstruction { get; set; }
        public EnPropertyType EnPropertyType { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
