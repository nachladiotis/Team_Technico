using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.Models;

namespace TechnicoRMP.Dtos;

public class CreatePropertyItemResponseService
{
    public static CreatePropertyItemResponse CreateFromEntity(PropertyItem propertyItem)
    {
        return new CreatePropertyItemResponse
        {
            Id = propertyItem.Id,
            E9Number = propertyItem.E9Number,
            Address = propertyItem.Address,
            EnPropertyType = propertyItem.EnPropertyType,
            IsActive = propertyItem.IsActive,
           YearOfConstruction = propertyItem.YearOfConstruction,
        };
    }
  

}
