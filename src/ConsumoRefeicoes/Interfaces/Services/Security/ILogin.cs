using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces.Services.Security
{
    public interface ILogin
    {
        Task<ResponseLogin> Authenticate(Login login);
    }
}