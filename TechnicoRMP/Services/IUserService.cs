using TechnicoRMP.Models;
using TechnicoRMP.Responses;

namespace TechnicoRMP.Servicesp;

public interface IUserService
{
    void Display(string vatNumber);
    Response Update(User user);
    bool Delete(string vatNumber);
    Response<User> Create();
}