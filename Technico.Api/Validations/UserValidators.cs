using TechnicoRMP.Models;

namespace Technico.Api.Validations;


public class UserValidators
{
    public static bool IsValidEmail(string email)
    {
        return email.Contains("@") && email.Contains(".");
    }

    public static bool IsValidVAT(string vat)
    {
        return vat.Length >= 9 && vat.Length <= 20;
    }

    public static bool IsValidPassword(string password)
    {
        return password.Length >= 8 && password.Any(char.IsDigit) && password.Any(char.IsLetter);
    }

    public static bool ValidateUserForCreate(User user)
    {
        if (!IsValidVAT(user.VatNumber))
        {
            return false;
        }

        if (string.IsNullOrEmpty(user.VatNumber) && user.Name != "string")
        {
            Console.WriteLine($"VAT number is required and cannot be empty.");
            return false;
        }

        if (!IsValidEmail(user.Email))
        {
            return false;
        }

        if (!IsValidPassword(user.Password))
        {
            return false;
        }

        return true;
    }
    public static bool ValidateUserForUpdate(User user)
    {
        if (!IsValidEmail(user.Email))
        {
            return false;
        }
        if (!IsValidPassword(user.Password))
        {
            return false;
        }
        if (!IsValidVAT(user.VatNumber))
        {
            return false;
        }
        return true;
    }
    public static bool ValidateUserForReplace(User user)
    {
        if (!IsValidVAT(user.VatNumber))
        {
            return false;
        }

        if (!IsValidEmail(user.Email))
        {
            return false;
        }
        if (!IsValidPassword(user.Password))
        {
            return false;
        }
        return true;
    }
}


