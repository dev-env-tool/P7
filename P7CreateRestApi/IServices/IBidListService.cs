using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;

namespace P7CreateRestApi.IServices
{
    public interface IBidListService
    {
        Task<IEnumerable<BidListDto>> GetAllBidListsDto();
        Task<IEnumerable<BidListDto>> GetBidListDtoById(int id);
        Task CreateBidListWithBidListDto(BidListDto BidListDto);
        Task UpdateBidListWithBidListDto(BidListDto BidListDto);
        Task DeleteBidListById(int id);
        Task<BidListDto> MapBidListToBidListDto(BidList BidList);
        Task<BidList> MapBidListDtoToBidList(BidListDto BidListDto);

    }
}