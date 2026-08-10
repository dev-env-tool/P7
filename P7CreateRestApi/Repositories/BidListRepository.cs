
using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.IRepositories;
using System.Collections;

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
            return await _context!.BidLists.Where(bL => bL.BidListId == id)
                                  .ToListAsync();
        }

        public void CreateBidList(BidList bidList)
        {
            if (bidList != null)
            {
                _context!.BidLists.Add(bidList);
                _context.SaveChanges();
            }
        }
        public async Task UpdateBidList(BidList bidList)
        {
            if (bidList != null)
            {
                _context.Entry(bidList).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }

        public void DeleteBidListById(int id)
        {
            BidList bidList = _context!.BidLists.First(b => b.BidListId == id);

            if (bidList != null)
            {
                _context!.BidLists.Remove(bidList);
                _context.SaveChanges();
            }
        }
    }
}