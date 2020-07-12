using BooksStore.Domain.Contracts.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace BooksStore.App.Client.Controllers
{
    public class DatabaseController : Controller
    {
        public IActionResult ShowInfo()
        {
            var model = new AdminToolbarViewModel {IsFormButtonVisible = false};

            return View("DatabaseInfo", model);
        }
    }
}