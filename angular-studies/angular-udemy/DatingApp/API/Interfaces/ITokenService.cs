using API.Entities;

namespace API.Interfaces
{
    public interface ITokenService
    {
        string CreateToken (AppUser user);
        bool ValidateToken (string token);
    }
}