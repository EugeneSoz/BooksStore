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
                .ForMember(d => d.CurrentPage,
                    opts => opts.MapFrom(DefineCurrentPage))
                .ForMember(d => d.OrderConditions, 
                    opts => 
                        opts.MapFrom(src => src.PropertyName == null 
                            ? new [] { new Condition(nameof(EntityBase.Id), nameof(SortingOrder.Asc).ToUpper()) }
                            : new [] { new Condition(src.PropertyName, src.Order)}))

                .ForMember(d => d.SearchConditions,
                    opts =>
                    {
                        opts.PreCondition(src => src.FilterPropertyName != null && src.FilterAction == FilterAction.Search);
                        opts.MapFrom(src => new[]
                        {
                            new Condition(src.FilterPropertyName, src.FilterPropertyValue)
                        });
                    });

            CreateMap<PublisherEntity, PublisherResponse>();
        }

        private int DefineCurrentPage(PageConditionsQuery source, QueryConditions destination)
        {
            if (source.FilterPropertyName != null)
            {
                source.CurrentPage = 1;

                return source.CurrentPage;
            }

            return source.CurrentPage == 0 ? 1 : source.CurrentPage;
        }
    }
}