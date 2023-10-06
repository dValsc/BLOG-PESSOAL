using BLOGPESSOAL.Model;

namespace BLOGPESSOAL.Security
{
    public interface IAuthService
    {
        Task<UserLogin?> Autenticar(UserLogin userLogin);
    }
}
