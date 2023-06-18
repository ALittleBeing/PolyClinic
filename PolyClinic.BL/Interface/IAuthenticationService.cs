using PolyClinic.Common.Models;

namespace PolyClinic.BL.Interface
{
    public interface IAuthenticationService
    {
        AuthenticationResponse CreateToken(string userName, string userEmail);
        Task<string> CreateUserAsync(User user);
        Task<string> ValidateLoginAsync(AuthenticationRequest request);
    }
}