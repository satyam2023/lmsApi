namespace lmsApi.Helpers;

using AutoMapper;
using lmsApi.Models.Dtos.BookCopy;
using lmsApi.Models.Dtos.IssuedBook;
using lmsApi.Models.Dtos.User;
using lmsApi.Models.Entities;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<CreateUser, User>().ReverseMap();
        CreateMap<UserDetail, User>().ReverseMap();
        CreateMap<CreateBookDto, Book>();
        CreateMap<Book, BookDetailDto>().ReverseMap();
        CreateMap<CreateBookCopyDto, BookCopy>();
        CreateMap<BookCopy, BookCopyDetailDto>();
        CreateMap<IssueBookDto, IssuedBook>().ReverseMap();
        CreateMap<CreateBookCategory, BookCategory>().ReverseMap();
        CreateMap<BookCategory, BookCategoryDetail>().ReverseMap();
        CreateMap<Book, BookCategoryResponse>().ReverseMap(); 
        CreateMap<Book, BookDetailForCategory>()
            .ForMember(dest => dest.AvailableCopies, opt => opt.MapFrom(src => src.AvailableCopies))
            .ForMember(dest => dest.IssuedCopies, opt => opt.MapFrom(src => src.IssuedCopies))
            .ForMember(dest => dest.HasAvailableCopies, opt => opt.MapFrom(src => src.HasAvailableCopies));
    }
}