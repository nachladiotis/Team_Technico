namespace TechnicoRMP.WebApp.Models
{
    public class UserProfileViewModelUpdate
    {
        public long Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? VatNumber { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        //public EnRoleType? TypeOfUser { get; set; }
        public string? Password { get; set; }
    }
}
