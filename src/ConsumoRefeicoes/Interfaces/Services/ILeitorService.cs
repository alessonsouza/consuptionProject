using System.Collections.Generic;

using backend.Models;

namespace backend.Interfaces.Services
{
    public interface ILeitorService
    {
        void Initialize();
        string Capture();
        bool Compare(UserDigital user);
        void RegisterUsersDigital(IEnumerable<UserDigital> listUsers);

        string Identify(string digital);
    }
}