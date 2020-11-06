using AutoMapper;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Persistence.Entities;

namespace BooksStore.App.Handlers.Mapping
{
    public class BookProfile : Profile
    {
        public BookProfile()
        {
            CreateMap<BookEntity, BookResponse>();
        }
    }
}