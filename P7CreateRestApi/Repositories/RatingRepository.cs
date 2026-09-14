using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using P7CreateRestApi.IRepositories;
using System.Collections;


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
                await _context!.Ratings.AddAsync(rating);
                await _context!.SaveChangesAsync();
            }
        }

        public async Task UpdateRating(Rating rating)
        {
            var existingRating = await _context.Ratings.FindAsync(rating.Id);

            if (existingRating != null)
            {
                _context.Entry(existingRating).CurrentValues.SetValues(rating);
                await _context!.SaveChangesAsync();
            }
        }


        public async Task DeleteRatingById(int id)
        {
            Rating rating = _context!.Ratings.First(r => r.Id == id);

            if (rating != null)
            {
                _context!.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
            }
        }
    }
}