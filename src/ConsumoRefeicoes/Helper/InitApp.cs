using System.Threading.Tasks;
using System.Collections.Generic;
using backend.Interfaces.Services;
using backend.Models;

namespace backend.Helper
{
    public class InitApp
    {
        private readonly ILeitorService _leitorService;
        public readonly IDigital _digitalService;

        private IEnumerable<UserDigital> allUsers;

        public InitApp(ILeitorService leitorService, IDigital digitalService)
        {
            _leitorService = leitorService;
            _digitalService = digitalService;

            _leitorService.Initialize();


            Task.Run(() => this.GetUsers()).Wait();




            _leitorService.RegisterUsersDigital(allUsers);

        }

        private async Task<IEnumerable<UserDigital>> GetUsers()
        {
            allUsers = await _digitalService.Comparar();

            return allUsers;
        }
    }
}