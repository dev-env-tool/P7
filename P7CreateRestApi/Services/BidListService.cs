using AutoMapper;
using Moq;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;


namespace P7CreateRestApi.Services
{
    public class BidListService : IBidListService
    {
        private readonly IBidListRepository _iBidListRepository;
        private readonly IMapper _mapper;
        //private IBidListRepository bidListRepository;
        //private Mock<IMapper> mockMapper;

        public BidListService(IBidListRepository iBidListRepository, IMapper iMapper)
        {
            _iBidListRepository = iBidListRepository;
            _mapper = iMapper;
        }

        //public BidListService(IBidListRepository bidListRepository, Mock<IMapper> mockMapper)
        //{
        //    this.bidListRepository = bidListRepository;
        //    this.mockMapper = mockMapper;
        //}

        public async Task<IEnumerable<BidListDto>> GetAllBidListsDto()
        {
            Task<IEnumerable<BidList>> bidLists = _iBidListRepository!.GetAllBidLists();
            List<BidListDto> ListOfBidListDtos = new List<BidListDto>();
            foreach (BidList bidList in await bidLists)
            {
                BidListDto bidListDto = await MapBidListToBidListDto(bidList);
                ListOfBidListDtos.Add(bidListDto);
            }

            return ListOfBidListDtos;
        }

        public async Task<IEnumerable<BidListDto>> GetBidListDtoById(int id)
        {
            Task<IEnumerable<BidList>> bidLists = _iBidListRepository!.GetBidListById(id);
            List<BidListDto> ListOfBidListDtos = new List<BidListDto>();
            foreach (BidList bidList in await bidLists)
            {
                BidListDto bidListDto = await MapBidListToBidListDto(bidList);
                ListOfBidListDtos.Add(bidListDto);
            }

            return ListOfBidListDtos;
        }

        public async Task CreateBidListWithBidListDto(BidListDto bidListDto)
        {
            BidList bidList = await MapBidListDtoToBidList(bidListDto);
            if (bidListDto != null)
            {
                await _iBidListRepository.CreateBidList(bidList);
            }
        }

        public async Task UpdateBidListWithBidListDto(BidListDto bidListDto)
        {
            BidList bidList = await MapBidListDtoToBidList(bidListDto);
            if (bidListDto != null)
            {
                await _iBidListRepository.UpdateBidList(bidList);
            }
        }


        public async Task DeleteBidListById(int id)
        {
            await _iBidListRepository.DeleteBidListById(id);
        }

        public async Task<BidListDto> MapBidListToBidListDto(BidList bidList)
        {
            BidListDto bidListDto = _mapper.Map<BidListDto>(bidList);
            return bidListDto;
        }
        public async Task<BidList> MapBidListDtoToBidList(BidListDto bidListDto)
        {
            BidList bidList = _mapper.Map<BidList>(bidListDto);
            return bidList;
        }



    }
}
