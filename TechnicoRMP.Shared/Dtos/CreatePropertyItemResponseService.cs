using TechnicoRMP.Models;

namespace TechnicoRMP.Shared.Dtos;

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

    public static PropertyItmesByUserDto Create(List<PropertyOwnership> propertyOwnerships) 
    {
        List< PropertyItemsDto > tmp = new List< PropertyItemsDto >();
        var owndership = propertyOwnerships.First();
        var userDto = new UserDto
        {
            Email = owndership.PropertyOwner!.Email,
            Address = owndership.PropertyOwner.Address,
            Id = owndership.PropertyOwner.Id,
            Name = owndership.PropertyOwner.Name,
            PhoneNumber = owndership.PropertyOwner.PhoneNumber,
            Surname = owndership.PropertyOwner.Surname,
            TypeOfUser = owndership.PropertyOwner.TypeOfUser,
            VatNumber = owndership.PropertyOwner.VatNumber
        };
        foreach (var propertyOwnership in propertyOwnerships) 
        {
            if(propertyOwnership.PropertyItem is null)
            {
                continue;
            }

            tmp.Add(new PropertyItemsDto
            {
                Address = propertyOwnership.PropertyItem.Address,
                E9Number = propertyOwnership.PropertyItem.E9Number,
                EnPropertyType = propertyOwnership.PropertyItem.EnPropertyType,
                IsActive = propertyOwnership.PropertyItem.IsActive,
                Id = propertyOwnership.PropertyItem.Id,
                YearOfConstruction  = propertyOwnership.PropertyItem.YearOfConstruction,
            });
        }

        return new PropertyItmesByUserDto
        {
            PropertyItems = tmp,
            USerDto = userDto
        };
    }

}

public sealed class PropertyItmesByUserDto : IDto
{
    public UserDto? USerDto { get; set; }

    public List<PropertyItemsDto> PropertyItems { get; set; } = [];
}

public sealed class PropertyItemsDto : IDto
{
    public long Id { get; set; }
    public string? E9Number { get; set; }
    public string? Address { get; set; }
    public int YearOfConstruction { get; set; }
    public EnPropertyType EnPropertyType { get; set; }
    public bool IsActive { get; set; }
}