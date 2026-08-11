//using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;

namespace P7CreateRestApi.IRepositories
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAllRatings();
        Task<IEnumerable<Rating>> GetRatingById(int id);
        void CreateRating(Rating rating);
        Task UpdateRating(Rating rating);
        void DeleteRatingById(int id);

    }
}