//using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;

namespace P7CreateRestApi.IRepositories
{
    public interface ITradeRepository
    {
        Task<IEnumerable<Trade>> GetAllTrades();
        Task<IEnumerable<Trade>> GetTradeById(int id);
        Task CreateTrade(Trade trade);
        Task UpdateTrade(Trade trade);
        Task DeleteTradeById(int id);

    }
}