using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces.Services
{
    public interface IUserGeral
    {
        Task<IEnumerable<Users>> GetUsers(string username);
    }
}