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
    }
}