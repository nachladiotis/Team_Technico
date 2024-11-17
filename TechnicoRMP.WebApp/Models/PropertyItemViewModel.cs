using System.ComponentModel;
using TechnicoRMP.Models;

namespace TechnicoRMP.WebApp.Models
{
    public class PropertyItemViewModel
    {
        public long Id { get; set; }

        [DisplayName("E9 Number")]
        public string E9Number { get; set; }
        public string Address { get; set; }
        [DisplayName("Year of Construction")]
        public int YearOfConstruction { get; set; }
        [DisplayName("Item Type")]
        public EnPropertyType EnPropertyType { get; set; }
        public bool IsActive { get; set; } = true;
        public long UserId { get; set; }

    }

    public class CreatePropertyItemViewmodel
    {
        public string? E9Number { get; set; }
        public string? Address { get; set; }
        public int YearOfConstruction { get; set; }
        public EnPropertyType EnPropertyType { get; set; }
        public bool IsActive { get; set; } = true;

        public long UserId { get; set; }
    }
}
