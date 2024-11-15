﻿using System.ComponentModel;
using TechnicoRMP.Models;

namespace TechnicoRMP.WebApp.Models
{
    public class PropertyItemViewModel
    {
        public long Id { get; set; }
        [DisplayName("VAT Number")]
        public string E9Number { get; set; }
        public string Address { get; set; }
        [DisplayName("Years of Construction")]
        public int YearOfConstruction { get; set; }
        [DisplayName("Item Type")]
        public EnPropertyType EnPropertyType { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
