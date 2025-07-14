namespace lmsApi.Helpers;

using AutoMapper;
using lmsApi.Models.Dtos.User;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateUserDto, User>().ReverseMap();
        CreateMap<UserDetail, User>().ReverseMap();
    }
}