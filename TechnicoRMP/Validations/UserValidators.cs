using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicoRMP.Models;

namespace TechnicoRMP.Validations;

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
            Console.WriteLine($"Invalid VAT: {user.VatNumber}");
            return false;
        }

        if (!IsValidEmail(user.Email))
        {
            Console.WriteLine($"Invalid Email: {user.Email}");
            return false;
        }

        if (!IsValidPassword(user.Password))
        {
            Console.WriteLine($"Weak Password: {user.Password}");
            return false;
        }

        return true;
    }
    public static bool ValidateUserForUpdate(User user)
    {
        if (!IsValidEmail(user.Email))
        {
            Console.WriteLine($"Invalid Email: {user.Email}");
            return false;
        }
        if (!IsValidPassword(user.Password))
        {
            Console.WriteLine($"Weak Password: {user.Password}");
            return false;
        }
        return true;
    }
}

