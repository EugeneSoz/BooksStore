using AutoMapper;
using BooksStore.Domain.Contracts.Models.Categories;
using BooksStore.Persistence.Entities;

namespace BooksStore.App.Handlers.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryEntity, CategoryResponse>();
        }
    }
}