using Order.ViewModels;

namespace Order.Interface
{
    public interface IJWTManagerRepository
    {
        string Authenticate(LoginViewModel login);
    }
}
