using System;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Query;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.App.Client.Components
{
    public class StoreSidebarViewComponent : ViewComponent
    {
        private readonly CategoryQueryHandler _categoryQueryHandler;

        public StoreSidebarViewComponent(CategoryQueryHandler categoryQueryHandler)
        {
            _categoryQueryHandler = categoryQueryHandler ?? throw new ArgumentNullException(nameof(categoryQueryHandler));
        }

        public IViewComponentResult Invoke() 
        {
            var query = new StoreCategoryQuery();
            var result = _categoryQueryHandler.Handle(query);

            return View(result);
        }
    }
}