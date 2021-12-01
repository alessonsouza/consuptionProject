using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces.Services
{
    public interface IUser
    {
        Task<IEnumerable<User>> GetSearchUser(int matricula);
        int GetRegister(int matricula, string competencia);
    }
}