using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;


namespace P7CreateRestApi.Repositories
{
    public class BidListRepository : IBidListRepository
    {

        private static P7Referential? _context;

        public BidListRepository(P7Referential context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BidList>> GetAllBidLists()
        {
            return await _context!.BidLists.ToListAsync();
        }

        public async Task<IEnumerable<BidList>> GetBidListById(int id)
        {
            return await _context!.BidLists.Where(b => b.BidListId == id)
                                  .ToListAsync();
        }

        public async Task CreateBidList(BidList bidList)
        {
            if (bidList != null)
            {
                await _context!.BidLists.AddAsync(bidList);
                await _context!.SaveChangesAsync();
            }
        }

        public async Task UpdateBidList(BidList bidList)
        {

            var existingProduct = await _context.BidLists.FindAsync(bidList.BidListId);

            if (existingProduct != null)
            {
                _context.Entry(existingProduct).CurrentValues.SetValues(bidList);
                await _context!.SaveChangesAsync();
            }
        }


        public async Task DeleteBidListById(int id)
        {
            BidList bidList = _context!.BidLists.First(b => b.BidListId == id);

            if (bidList != null)
            {
                _context!.BidLists.Remove(bidList);
                await _context.SaveChangesAsync();
            }
        }
    }
}