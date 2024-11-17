
namespace TechnicoRMP.WebApp.Models
{
    public class RegisterModel 
    {
        public required string Name { get; set; }
        public required string VatNumber { get; set; }
        public required string Surname { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string ConfirmPassword { get; set; }
    }
}
