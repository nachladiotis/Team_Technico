using TechnicoRMP.Models;

namespace TechnicoMVCApp.Models
{
    public class PropertyItemViewModel
    {
        public long Id { get; set; }
        public string E9Number { get; set; }
        public string Address { get; set; }
        public int YearOfConstruction { get; set; }
        public EnPropertyType EnPropertyType { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
