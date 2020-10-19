using AutoMapper;
using BooksStore.Domain.Contracts.Models.Categories;
using BooksStore.Persistence.Entities;

namespace BooksStore.App.Handlers.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryEntity, CategoryResponse>()
                .ForMember(d => d.DisplayedName,
                    opts => 
                        opts.MapFrom(src => src.ParentAndChildName));
        }
    }
}