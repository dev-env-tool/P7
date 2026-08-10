using Dot.Net.WebApi.Controllers.Domain;
using P7CreateRestApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

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