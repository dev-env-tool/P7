using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.IRepositories;
using P7CreateRestApi.IServices;
using P7CreateRestApi.Repositories;


namespace P7CreateRestApi.Services
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _iRatingRepository;
        private readonly IMapper _mapper;


        public RatingService(IRatingRepository iRatingRepository, IMapper iMapper)
        {
            _iRatingRepository = iRatingRepository;
            _mapper = iMapper;
        }
        public async Task<IEnumerable<RatingDto>> GetAllRatingsDto()
        {
            Task<IEnumerable<Rating>> ratings = _iRatingRepository!.GetAllRatings();
            List<RatingDto> ListOfratingDtos = new List<RatingDto>();
            foreach (Rating rating in await ratings)
            {
                RatingDto ratingDto = await MapRatingToRatingDto(rating);
                ListOfratingDtos.Add(ratingDto);
            }

            return ListOfratingDtos;
        }

        public async Task<IEnumerable<RatingDto>> GetRatingDtoById(int id)
        {
            Task<IEnumerable<Rating>> ratings = _iRatingRepository!.GetRatingById(id);
            List<RatingDto> ListOfratingDtos = new List<RatingDto>();
            foreach (Rating rating in await ratings)
            {
                RatingDto ratingDto = await MapRatingToRatingDto(rating);
                ListOfratingDtos.Add(ratingDto);
            }

            return ListOfratingDtos;
        }

        public async Task CreateRatingWithRatingDto(RatingDto ratingDto)
        {
            Rating rating = await MapRatingDtoToRating(ratingDto);
            if (ratingDto != null)
            {
                await _iRatingRepository.CreateRating(rating);
            }
        }

        public async Task UpdateRatingWithRatingDto(RatingDto ratingDto)
        {
            Rating rating = await MapRatingDtoToRating(ratingDto);
            if (ratingDto != null)
            {
                await _iRatingRepository.UpdateRating(rating);
            }
            //int maxRatingId = await GetMaxRatingId();
            //if (rating != null)
            //{
            //    if ((rating.Id > 0) && (rating.Id <= maxRatingId))
            //    {
            //        _context!.Entry(rating).State = EntityState.Modified;
            //        _context.SaveChanges();
            //    }
            //}
        }


        public async Task DeleteRatingById(int id)
        {
            await _iRatingRepository.DeleteRatingById(id);
        }

        public async Task<RatingDto> MapRatingToRatingDto(Rating rating)
        {
            RatingDto ratingDto = _mapper.Map<Rating, RatingDto>(rating);
            return ratingDto;
        }
        public async Task<Rating> MapRatingDtoToRating(RatingDto ratingDto)
        {
            Rating rating = _mapper.Map<RatingDto, Rating>(ratingDto);
            return rating;
        }



    }
}
