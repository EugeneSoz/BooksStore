using System;
using BooksStore.App.Contracts.Command;
using BooksStore.App.Handlers.Mapping;
using BooksStore.Domain.Contracts.Repositories;
using BooksStore.Persistence.Entities;
using OnlineBooksStore.App.Handlers.Interfaces;

namespace BooksStore.App.Handlers.Command
{
    public class CategoryCommandHandler : ICommandHandler<CreateCategoryCommand, CategoryEntity>,
        ICommandHandler<UpdateCategoryCommand, bool>,
        ICommandHandler<DeleteCategoryCommand, bool>
    {
        private readonly ICategoriesRepository _categoriesRepository;

        public CategoryCommandHandler(ICategoriesRepository categoriesRepository)
        {
            _categoriesRepository = categoriesRepository ?? throw new ArgumentNullException(nameof(categoriesRepository));
        }

        public CategoryEntity Handle(CreateCategoryCommand command)
        {
            var category = command.MapCategoryEntity();
            return _categoriesRepository.AddCategory(category);
        }

        public bool Handle(UpdateCategoryCommand command)
        {
            var category = command.MapCategoryEntity();
            return _categoriesRepository.UpdateCategory(category);
        }

        public bool Handle(DeleteCategoryCommand command)
        {
            var category = command.MapCategoryEntity();

            return _categoriesRepository.DeleteCategory(category);
        }
    }
}