
//using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.IRepositories;

namespace P7CreateRestApi.Repositories
{
    public class TradeRepository : ITradeRepository
    {

        private static P7Referential? _context;

        public TradeRepository(P7Referential context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Trade>> GetAllTrades()
        {
            return await _context!.Trades.ToListAsync();
        }

        public async Task<IEnumerable<Trade>> GetTradeById(int id)
        {
            return await _context!.Trades.Where(t => t.TradeId == id)
                                  .ToListAsync();
        }

        public void CreateTrade(Trade trade)
        {
            if (trade != null)
            {
                _context!.Trades.Add(trade);
                _context.SaveChanges();
            }
        }
        public async Task UpdateTrade(Trade trade)
        {
            int maxTradeId = await GetMaxTradeId();
            if (trade != null)
            {
                if ((trade.TradeId > 0) && (trade.TradeId <= maxTradeId))
                {
                    _context!.Entry(trade).State = EntityState.Modified;
                    _context.SaveChanges();

                }
            }
        }

        public void DeleteTradeById(int id)
        {
            Trade trade = _context!.Trades.First(t => t.TradeId == id);

            if (trade != null)
            {
                _context!.Trades.Remove(trade);
                _context.SaveChanges();
            }
        }

        private static async Task<int> GetMaxTradeId()
        {
            int maxTradeId = _context!.Trades.Select(t => t.TradeId).Max();
            return await Task.FromResult(maxTradeId);
        }
    }
}