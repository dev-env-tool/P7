//using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;

namespace P7CreateRestApi.IRepositories
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAllRatings();
        Task<IEnumerable<Rating>> GetRatingById(int id);
        Task CreateRating(Rating rating);
        Task UpdateRating(Rating rating);
        Task DeleteRatingById(int id);

    }
}