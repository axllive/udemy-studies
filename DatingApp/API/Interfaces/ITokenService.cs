using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        Task<string> CreateToken (AppUser user);
        bool ValidateToken (string token);
    }
}