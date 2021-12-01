using System.Collections.Generic;
using System.Threading.Tasks;
using backend.Models;

namespace backend.Interfaces.Services
{
    public interface IRefeicoes
    {
        Task<IEnumerable<ShowFoodItems>> GetSearchItem(int codigo);

        Task<IEnumerable<ShowFoodItems>> GetAllFood(string competencia);
        Task<IEnumerable<ShowFoodItems>> GetFoodUnusedService(string ids2);

        // bool SaveOrder(ReceiveFoodItems post);
        object SaveOrder(FoodItems post);
        Task<IEnumerable<FoodItems>> GetMealById(int matricula, string competencia);

    }
}