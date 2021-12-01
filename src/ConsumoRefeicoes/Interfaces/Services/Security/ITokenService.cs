using backend.Models;

namespace backend.Interfaces.Services.Security
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}