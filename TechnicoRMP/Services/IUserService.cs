using TechnicoRMP.Models;

namespace TechnicoRMP.Servicesp;

public interface IUserService
{
    void Display(string vatNumber);
    void Update(User user);
    bool Delete(string vatNumber);
    User Create();
}