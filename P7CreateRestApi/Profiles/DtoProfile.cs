using AutoMapper;
using P7CreateRestApi.Domain;
using P7CreateRestApi.DTO;
using P7CreateRestApi.Models;


namespace P7CreateRestApi.Profiles
{
    public class DtoProfile : Profile
    {

        public DtoProfile() 
        {
            CreateMap<BidList, BidListDto>();
            CreateMap<BidListDto, BidList>();
            CreateMap<CurvePoint, CurvePointDto>();
            CreateMap<CurvePointDto, CurvePoint>();
            CreateMap<Rating, RatingDto>();
            CreateMap<RatingDto, Rating>();
            CreateMap<RuleName, RuleNameDto>();
            CreateMap<RuleNameDto, RuleName>();
            CreateMap<Trade, TradeDto>();
            CreateMap<TradeDto, Trade>();
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<UpdateGeneralInfosModel, User>();
            CreateMap<RegisterModel, User>();
        }



    }
}
