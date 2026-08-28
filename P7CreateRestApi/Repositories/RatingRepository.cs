//using Dot.Net.WebApi.Controllers.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IRepositories;


namespace P7CreateRestApi.Repositories
{
    public class RatingRepository : IRatingRepository
    {

        private static P7Referential? _context;

        public RatingRepository(P7Referential context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rating>> GetAllRatings()
        {
            return await _context!.Ratings.ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingById(int id)
        {
            return await _context!.Ratings.Where(r => r.Id == id)
                                  .ToListAsync();
        }

        public async Task CreateRating(Rating rating)
        {
            if (rating != null)
            {
                _context!.Ratings.Add(rating);
                _context.SaveChanges();
            }
        }

        public async Task UpdateRating(Rating rating)
        {
            int maxRatingId = await GetMaxRatingId();
            if (rating != null)
            {
                if ((rating.Id > 0) && (rating.Id <= maxRatingId))
                {
                    _context!.Entry(rating).State = EntityState.Modified;
                    _context.SaveChanges();
                }
            }
        }


        public async Task DeleteRatingById(int id)
        {
            Rating rating = _context!.Ratings.First(r => r.Id == id);

            if (rating != null)
            {
                _context!.Ratings.Remove(rating);
                _context.SaveChanges();
            }
        }
        private static async Task<int> GetMaxRatingId()
        {
            int maxRatingId = _context!.Ratings.Select(r => r.Id).Max();
            return await Task.FromResult(maxRatingId);
        }
    }
}