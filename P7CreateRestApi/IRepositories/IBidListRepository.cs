//using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;


namespace P7CreateRestApi.IRepositories
{
    public interface IBidListRepository
    {
        Task<IEnumerable<BidList>> GetAllBidLists();
        Task<IEnumerable<BidList>> GetBidListById(int id);
        Task CreateBidList(BidList bidList);
        Task UpdateBidList(BidList bidList);
        Task DeleteBidListById(int id);

    }
}