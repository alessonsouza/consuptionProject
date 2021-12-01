using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces.Services
{
    public interface IDigital
    {
        Task<IEnumerable<UserDigital>> Comparar();

        IEnumerable<UserDigital> GetAllUsers();
        Task<UserDigital> GetUserByNumCad(string numcad);
    }
}