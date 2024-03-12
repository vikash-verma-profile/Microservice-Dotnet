using Inventory.ViewModels;

namespace Inventory.Interface
{
    public interface IJWTManagerRepository
    {
        string Authenticate(LoginViewModel login);
    }
}
