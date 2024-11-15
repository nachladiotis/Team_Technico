namespace TechnicoRMP.Shared.Dtos
{
    public class UserWithProperyItemsDTO : UserDto
    {
        public List<CreatePropertyItemRequest>? PropertyItems { get; set; }
    }
}
