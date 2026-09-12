
//using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using System.Collections;

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

        public async Task CreateTrade(Trade trade)
        {
            if (trade != null)
            {
                await _context!.Trades.AddAsync(trade);
                await _context!.SaveChangesAsync();
            }
        }
        public async Task UpdateTrade(Trade trade)
        {
            var existingTrade = await _context.Trades.FindAsync(trade.TradeId);

            if (existingTrade != null)
            {
                _context.Entry(existingTrade).CurrentValues.SetValues(trade);
                await _context!.SaveChangesAsync();
            }
        }

        public async Task DeleteTradeById(int id)
        {
            Trade trade = _context!.Trades.First(t => t.TradeId == id);

            if (trade != null)
            {
                _context!.Trades.Remove(trade);
                await _context.SaveChangesAsync();
            }
        }
    }
}