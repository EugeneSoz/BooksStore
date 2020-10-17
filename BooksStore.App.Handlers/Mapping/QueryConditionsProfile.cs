using System.Collections.Generic;
using AutoMapper;
using BooksStore.App.Contracts.Query;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Properties;

namespace BooksStore.App.Handlers.Mapping
{
    public class QueryConditionsProfile : Profile
    {
        public QueryConditionsProfile()
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
                        opts.PreCondition(src => src.SearchValue != null && src.FormAction == FormAction.Search);
                        opts.MapFrom(src => new[]
                        {
                            new Condition(src.SelectedPropertyName, src.SearchValue)
                        });
                    })
                
                .ForMember(d => d.FilterConditions,
                    opts =>
                    {
                        opts.PreCondition(src => (src.FormAction != FormAction.Cancel) && src.FirstRangeValue != null || src.SecondRangeValue != null);
                        opts.MapFrom(GetFilterConditions);
                    });
        }

        private int DefineCurrentPage(PageConditionsQuery source, QueryConditions destination)
        {
            if (source.SelectedPropertyName != null)
            {
                source.CurrentPage = 1;

                return source.CurrentPage;
            }

            return source.CurrentPage == 0 ? 1 : source.CurrentPage;
        }

        private Condition[] GetFilterConditions(PageConditionsQuery source, QueryConditions destination)
        {
            var conditions = new List<Condition>();
            if (source.FirstRangeValue != null)
            {
                conditions.Add(new Condition(source.SelectedPropertyName, source.FirstRangeValue));
            }

            if (source.SecondRangeValue != null)
            {
                conditions.Add(new Condition(source.SelectedPropertyName, source.SecondRangeValue));
            }

            return conditions.ToArray();
        }
    }
}