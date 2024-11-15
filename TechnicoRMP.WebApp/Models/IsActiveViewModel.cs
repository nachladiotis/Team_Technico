namespace TechnicoRMP.WebApp.Models
{
    public class IsActiveViewModel
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
