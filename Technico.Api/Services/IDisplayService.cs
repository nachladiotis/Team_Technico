namespace Technico.Api.Services
{
    public interface IDisplayService<T>
    {
        void Display(T indentifier);
        void DisplayAll();
    }
}