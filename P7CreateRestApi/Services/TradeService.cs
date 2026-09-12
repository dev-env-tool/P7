using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;


namespace P7CreateRestApi.Services
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _iTradeRepository;
        private readonly IMapper _mapper;


        public TradeService(ITradeRepository iTradeRepository, IMapper iMapper)
        {
            _iTradeRepository = iTradeRepository;
            _mapper = iMapper;
        }
        public async Task<IEnumerable<TradeDto>> GetAllTradesDto()
        {
            Task<IEnumerable<Trade>> trades = _iTradeRepository!.GetAllTrades();
            List<TradeDto> listOfTradeDtos = new List<TradeDto>();
            foreach (Trade trade in await trades)
            {
                TradeDto tradeDto = await MapTradeToTradeDto(trade);
                listOfTradeDtos.Add(tradeDto);
            }

            return listOfTradeDtos;
        }

        public async Task<IEnumerable<TradeDto>> GetTradeDtoById(int id)
        {
            Task<IEnumerable<Trade>> trades = _iTradeRepository!.GetTradeById(id);
            List<TradeDto> listOfTradeDtos = new List<TradeDto>();
            foreach (Trade trade in await trades)
            {
                TradeDto tradeDto = await MapTradeToTradeDto(trade);
                listOfTradeDtos.Add(tradeDto);
            }

            return listOfTradeDtos;
        }

        public async Task CreateTradeWithTradeDto(TradeDto tradeDto)
        {
            Trade trade = await MapTradeDtoToTrade(tradeDto);
            if (tradeDto != null)
            {
                await _iTradeRepository.CreateTrade(trade);
            }
        }

        public async Task UpdateTradeWithTradeDto(TradeDto tradeDto)
        {
            Trade trade = await MapTradeDtoToTrade(tradeDto);
            if (tradeDto != null)
            {
                await _iTradeRepository.UpdateTrade(trade);
            }
        }


        public async Task DeleteTradeById(int id)
        {
            await _iTradeRepository.DeleteTradeById(id);
        }

        public async Task<TradeDto> MapTradeToTradeDto(Trade trade)
        {
            TradeDto tradeDto = _mapper.Map<TradeDto>(trade);
            return tradeDto;
        }
        public async Task<Trade> MapTradeDtoToTrade(TradeDto tradeDto)
        {
            Trade trade = _mapper.Map<Trade>(tradeDto);
            return trade;
        }



    }
}

