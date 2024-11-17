namespace TechnicoRMP.WebApp.Models;

public class FilterUsersRequest
{
    public string? VatNumber { get; set; }
    public List<UserViewmodel> AllUsers { get; set; } = [];
}