using BooksStore.App.Client.Controllers;
using BooksStore.Common;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace BooksStore.App.Client.Infrastructure
{
    internal static class RoutesConfigurator
    {
        internal static IEndpointRouteBuilder MapPublisherControllerRoute(this IEndpointRouteBuilder endpoints)
        {
            var controller = ControllerHelper.GetName(nameof(PublishersController));
            var action = nameof(PublishersController.ShowPublishers);

            endpoints.MapControllerRoute(null,
                "Publishers/Page{Page}/{PropertyName}/{Order}", new { controller, action });

            endpoints.MapControllerRoute(null,
                "Publishers/Page{Page}", new { controller, action });

            endpoints.MapControllerRoute("pub",
                "Publisher/CreateOrEdit/{Id:long}",
                new
                {
                    controller, action = "CreatePublisher"
                });

            return endpoints;
        }

        internal static IEndpointRouteBuilder MapCategoriesControllerRoute(this IEndpointRouteBuilder endpoints)
        {
            var controller = ControllerHelper.GetName(nameof(CategoriesController));
            var action = nameof(CategoriesController.ShowCategories);

            endpoints.MapControllerRoute(null,
                "Categories/Page{Page}/{PropertyName}/{Order}", new {controller, action });

            endpoints.MapControllerRoute(null,
                "Categories/Page{Page}", new {controller, action });

            endpoints.MapControllerRoute("pub",
                "Category/CreateOrEdit/{Id:long}",
                new {controller = "Categories", action = "CreateCategory"});

            return endpoints;
        }

        internal static IEndpointRouteBuilder MapBooksControllerRoute(this IEndpointRouteBuilder endpoints)
        {
            var controller = ControllerHelper.GetName(nameof(BooksController));
            var action = nameof(BooksController.ShowBooks);

            endpoints.MapControllerRoute(null,
                "Books/Page{Page}/{PropertyName}/{Order}", new {controller, action });

            endpoints.MapControllerRoute(null,
                "Books/Page{Page}", new {controller = "Books", action = "BooksCategories"});
            endpoints.MapControllerRoute("pub",
                "Category/CreateOrEdit/{Id:long}",
                new {controller = "Categories", action = "CreateCategory"});

            return endpoints;
        }

        internal static IEndpointRouteBuilder MapStoreControllerRoute(this IEndpointRouteBuilder endpoints)
        {
            var controller = ControllerHelper.GetName(nameof(StoreController));
            var action = nameof(StoreController.ShowBooks);

            endpoints.MapControllerRoute("catpage",
                "{Category}/Page{Page}/{PropertyName}/{Order}",
                new { controller, action });

            endpoints.MapControllerRoute("page", "Page{Page}/{PropertyName}/{Order}",
                new { controller, action, page = 1 });

            endpoints.MapControllerRoute("category", "{Category}/{PropertyName}/{Order}",
                new { controller, action, page = 1 });

            endpoints.MapControllerRoute("catpage1",
                "{Category}/Page{Page}", new { controller, action });

            endpoints.MapControllerRoute("page1", "Page{Page}",
                new { controller, action, page = 1 });

            endpoints.MapControllerRoute("category1", "{Category}",
                new { controller, action, page = 1 });


            endpoints.MapControllerRoute("storeBookDetails",
                "Books/Details/{bookId:long}",
                new { Controller = "Store", action = "ShowDetails" });
            endpoints.MapControllerRoute("pagination",
                "Books/Page{page}",
                new { Controller = "Store", action = "ShowBooks" });

            return endpoints;
        }

    }
}