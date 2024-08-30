using AutoMapper;
using GamingPlatformBackend.Core.Models;
using GamingPlatformBackend.DTOs;

namespace GamingPlatformBackend;
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<User,UserLoginRequest>()
            .ReverseMap();

        CreateMap<User, UserRegistrationRequest>().ReverseMap();
        CreateMap<User, UserDTO>().ReverseMap();
        CreateMap<Game, GameDTO>().ReverseMap();    
        CreateMap<Game, GameBackDTO>().ReverseMap();    
        CreateMap<Score, ScoreDTO>().ReverseMap();
    }
}