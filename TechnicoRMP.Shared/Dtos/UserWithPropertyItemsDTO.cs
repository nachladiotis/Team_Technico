namespace TechnicoRMP.Shared.Dtos
{
    public class UserWithProperyItemsDTO : CreateUserResponse
    {
        public List<CreatePropertyItemRequest>? PropertyItems { get; set; }
    }
}
