using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Data;
using P7CreateRestApi.Domain;
using Microsoft.EntityFrameworkCore;
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
            return await _context!.Ratings.Where(bL => bL.Id == id)
                                  .ToListAsync();
        }

        public void CreateRating(Rating rating)
        {
            if (rating != null)
            {
                _context!.Ratings.Add(rating);
                _context.SaveChanges();
            }
        }

        public async Task UpdateRating(Rating rating)
        {
            if (rating != null)
            {
                _context.Entry(rating).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }


        public void DeleteRatingById(int id)
        {
            Rating rating = _context!.Ratings.First(b => b.Id == id);

            if (rating != null)
            {
                _context!.Ratings.Remove(rating);
                _context.SaveChanges();
            }
        }
    }
}