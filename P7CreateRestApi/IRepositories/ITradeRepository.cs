using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P7CreateRestApi.IRepositories
{
    public interface ITradeRepository
    {
        Task<IEnumerable<Trade>> GetAllTrades();
        Task<IEnumerable<Trade>> GetTradeById(int id);
        void CreateTrade(Trade trade);
        Task UpdateTrade(Trade trade);
        void DeleteTradeById(int id);

    }
}