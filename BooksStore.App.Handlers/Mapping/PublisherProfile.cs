using AutoMapper;
using BooksStore.App.Contracts.Query;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;
using BooksStore.Domain.Contracts.Models.Publishers;
using BooksStore.Persistence.Entities;

namespace BooksStore.App.Handlers.Mapping
{
    public class PublisherProfile : Profile
    {
        public PublisherProfile()
        {
            CreateMap<PageConditionsQuery, QueryConditions>()
                .ForMember(d => d.OrderConditions, 
                    opts => 
                        opts.MapFrom(src => src.PropertyName == null 
                            ? new [] { new Condition {PropertyName = nameof(EntityBase.Id), PropertyValue = nameof(SortingOrder.Asc).ToUpper() } }
                            : new [] { new Condition
                            {
                                PropertyName = src.PropertyName,
                                PropertyValue = src.Order
                            } }));

            CreateMap<PublisherEntity, PublisherResponse>();
        }
    }
}