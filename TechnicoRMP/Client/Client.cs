
using TechnicoRMP.Models;
using TechnicoRMP.Services;


namespace TechnicoRMP.Client;

public static class Client
{

    public static void DisplayAllUsers()
    {
        var displayUsers = new DisplayUserService(new DataAccess.DataStore());
        displayUsers.DisplayAll();
    }

  public static void CreateUser()
    {

        Console.Write("ΟΝΟΜΑ: ");
        string name = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(name))
        {
            Console.Write("ΤΟ ΟΝΟΜΑ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return;
        }

        Console.Write("ΕΠΩΝΥΜΟ: ");
        string surname = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(surname))
        {
            Console.Write("ΤΟ ΕΠΙΘΕΤΟ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return ;
        }

        Console.Write("Α.Φ.Μ.: ");
        string vatNumber = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(vatNumber))
        {
            Console.Write("ΤΟ ΑΦΜ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return;
        }

        Console.Write("ΔΙΕΥΘΥΝΣΗ (ΠΡΟΑΙΡΕΤΙΚΟ): ");
        string? address = Console.ReadLine();

        Console.Write("ΑΡΙΘΜΟΣ ΤΗΛΕΦΩΝΟΥ (ΠΡΟΑΙΡΕΤΙΚΟ): ");
        string? phoneNumber = Console.ReadLine();

        Console.Write("EMAIL: ");
        string email = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(email))
        {
            Console.Write("ΤΟ EMAIL ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return;
        }

        Console.Write("ΚΩΔΙΚΟΣ: ");
        string password = Console.ReadLine() ?? string.Empty;
        if (string.IsNullOrEmpty(password) || password.Length < 8)
        {
            Console.Write("Ο ΚΩΔΙΚΟΣ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟΣ ΚΑΙ ΠΡΕΠΕΙ ΝΑ ΕΙΝΑΙ ΤΟΥΛΑΧΙΣΤΟΝ 8 ΧΑΡΑΚΤΗΡΕΣ");
            return;
        }

        Console.Write("ΤΥΠΟΣ ΧΡΗΣΤΗ (1 ΓΙΑ ΠΕΛΑΤΗΣ, 2 ΓΙΑ ΕΠΙΣΚΕΥΑΣΤΗΣ): ");
  
        EnUserType userType = (Console.ReadLine() == "2") ? EnUserType.Provider : EnUserType.Customer;

        var userService = new UserService(new DataAccess.DataStore());
        var response = userService.Create(new Dtos.CreatUserRequest { Name = name,
        Surname = surname,
        VatNumber = vatNumber,
        Address = address,
        PhoneNumber = phoneNumber,
        Email = email,
        Password = password,
        });
        if (response.Status >= 0)
        {
            Console.WriteLine("SUCCESS");
        }
        else
        {
            Console.WriteLine("FAIL");
        }
    
    }

    public static void CreateItem()
    {
        Console.WriteLine("ΕΙΣΑΓΕΤΕ ΤΑ ΣΤΟΙΧΕΙΑ ΤΟΥ ΑΚΙΝΗΤΟΥ:");

        Console.Write("ΑΡΙΘΜΟΣ Ε9: ");
        string e9Number = Console.ReadLine() ?? string.Empty;
        if (e9Number == string.Empty)
        {
           Console.WriteLine("ΤΟ Ε9 ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return;
        }

        Console.Write("ΔΙΕΥΘΥΝΣΗ: ");
        string address = Console.ReadLine() ?? string.Empty;
         if (address == string.Empty)
        {
            Console.WriteLine("Η ΔΙΕΥΘΥΝΣΗ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΗ");
            return;
        }

        Console.Write("ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ: ");
        string yearOfConstruction = Console.ReadLine() ?? string.Empty;
        if (yearOfConstruction == string.Empty)
        {
            Console.WriteLine("ΤΟ ΕΤΟΣ ΚΑΤΑΣΚΕΥΗΣ ΕΙΝΑΙ ΥΠΟΧΡΕΩΤΙΚΟ");
            return;
        }

        bool res = int.TryParse(yearOfConstruction, out int year);
        if(!res || year > DateTime.Now.Year || year < 0)
        {
            Console.WriteLine("Η ΧΡΟΝΙΑ ΕΙΝΑΙ ΛΑΘΟΣ");
            return;
        }
   

        Console.WriteLine("ΤΥΠΟΣ ΑΚΙΝΗΤΟΥ (1 για ΔΙΑΜΕΡΙΣΜΑ, 2 για ΜΟΝΟΚΑΤΟΙΚΙΑ, 3 για ΜΕΖΟΝΕΤΑ): ");
        EnPropertyType propertyType = EnPropertyType.Apartment; 
        switch (Console.ReadLine())
        {
            case "1":
                propertyType = EnPropertyType.Apartment;
                break;
            case "2":
                propertyType = EnPropertyType.DetachedHouse;
                break;
            case "3":
                propertyType = EnPropertyType.Maisonet;
                break;
            default:
                Console.WriteLine("ΑΚΥΡΗ ΕΠΙΛΟΓΗ. Ο ΤΥΠΟΣ ΑΚΙΝΗΤΟΥ ΘΑ ΟΡΙΣΤΕΙ ΣΕ ΔΙΑΜΕΡΙΣΜΑ");
                return;
            
        }
        var propertyService = new PropertyItemService(new DataAccess.DataStore());
        var response = propertyService.Create(new Dtos.CreatePropertyItemRequest
        {
            E9Number = e9Number,
            Address = address,
            YearOfConstruction = year,
            EnPropertyType = propertyType,

        });
        if (response.Status >= 0)
        {
            Console.WriteLine("SUCCESS");
        }
        else
        {
            Console.WriteLine("FAIL");
        }
    }


}
