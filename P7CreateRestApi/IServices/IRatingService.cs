using Microsoft.AspNetCore.Mvc;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;

namespace P7CreateRestApi.IServices
{
    public interface IRatingService
    {
        Task<IEnumerable<RatingDto>> GetAllRatingsDto();
        Task<IEnumerable<RatingDto>> GetRatingDtoById(int id);
        Task CreateRatingWithRatingDto(RatingDto ratingDto);
        Task UpdateRatingWithRatingDto(RatingDto ratingDto);
        Task DeleteRatingById(int id);
        Task<RatingDto> MapRatingToRatingDto(Rating rating);
        Task<Rating> MapRatingDtoToRating(RatingDto ratingDto);

    }
}