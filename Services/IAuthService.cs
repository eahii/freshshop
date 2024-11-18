using System.Threading.Tasks;
using UsedPhonesWebShop.Shared.DTOs;

namespace UsedPhonesWebShop.Services
{
    public interface IAuthService
    {
        Task<bool> Register(UserRegistrationDto registrationDto);
        Task<string> Login(UserLoginDto loginDto);
        Task Logout();
    }
}