using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;

namespace P7CreateRestApi.IServices
{
    public interface ITradeService
    {
        Task<IEnumerable<TradeDto>> GetAllTradesDto();
        Task<IEnumerable<TradeDto>> GetTradeDtoById(int id);
        Task CreateTradeWithTradeDto(TradeDto TradeDto);
        Task UpdateTradeWithTradeDto(TradeDto TradeDto);
        Task DeleteTradeById(int id);
        Task<TradeDto> MapTradeToTradeDto(Trade Trade);
        Task<Trade> MapTradeDtoToTrade(TradeDto TradeDto);

    }
}