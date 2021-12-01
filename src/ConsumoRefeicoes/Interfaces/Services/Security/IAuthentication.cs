using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces.Services.Security
{
    public interface IAuthentication
    {
        Task<User> Autenthicate(string username, string password);
        bool BelongToGroup(string groupName);

    }
}