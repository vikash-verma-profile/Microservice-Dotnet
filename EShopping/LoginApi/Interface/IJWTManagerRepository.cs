using LoginApi.ViewModels;

namespace LoginApi.Interface
{
    public interface IJWTManagerRepository
    {
        string Authenticate(LoginViewModel login);
    }
}
