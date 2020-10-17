using AutoMapper;
using BooksStore.Domain.Contracts.Models.Publishers;
using BooksStore.Persistence.Entities;

namespace BooksStore.App.Handlers.Mapping
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<PublisherEntity, PublisherResponse>();
        }
    }
}