//using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;


namespace P7CreateRestApi.IRepositories
{
    public interface IBidListRepository
    {
        Task<IEnumerable<BidList>> GetAllBidLists();
        Task<IEnumerable<BidList>> GetBidListById(int id);
        void CreateBidList(BidList bidList);
        Task UpdateBidList(BidList bidList);
        void DeleteBidListById(int id);

    }
}