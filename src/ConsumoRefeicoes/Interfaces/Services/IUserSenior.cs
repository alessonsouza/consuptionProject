using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces.Services
{
    public interface IUserSenior
    {
        Task<IEnumerable<Users>> GetUsers(string username);
    }
}