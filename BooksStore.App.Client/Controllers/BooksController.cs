using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Command;
using BooksStore.App.Handlers.Query;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Books;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;
using OnlineBooksStore.App.Contracts.Command;

namespace BooksStore.App.Client.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookQueryHandler _queryHandler;
        private readonly BookCommandHandler _commandHandler;
        private readonly IUrlHelper _urlHelper;

        public BooksController(
            BookQueryHandler queryHandler, 
            BookCommandHandler commandHandler, 
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
            _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        [HttpGet]
        [HttpPost]
        public IActionResult ShowBooks(AdminFilter adminFilter, PageOptions pageOptions)
        {
            var query = new PageConditionsQuery(adminFilter, pageOptions);

            var model = _queryHandler.Handle(query);
            model.ToolbarViewModel.FormUrl = _urlHelper.Action("CreateBook", "Books", new { Id = 0 });

            return View("BooksSection", model);
        }

        [HttpGet("book/{id}")]
        public BookResponse GetBook([FromQuery] BookIdQuery query)
        {
            return _queryHandler.Handle(query);
        }


        [HttpPost("create")]
        public ActionResult CreateBook([FromBody] CreateBookCommand command)
        {
            if (!ModelState.IsValid)
            {
                return Ok(GetServerErrors(ModelState));
            }

            BookEntity book;
            try
            {
                book = _commandHandler.Handle(command);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Невозможно создать запись: {ex.Message}");
                return BadRequest(GetServerErrors(ModelState));
            }

            return Created("", book);
        }

        [HttpPut("update")]
        public ActionResult UpdateBook([FromBody] UpdateBookCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetServerErrors(ModelState));
            }

            bool isOk;
            try
            {
                isOk = _commandHandler.Handle(command);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Невозможно сохранить запись: {ex.Message}");
                return BadRequest(GetServerErrors(ModelState));
            }

            if (!isOk)
            {
                return NotFound();
            }
            return Ok();
        }

        [HttpDelete("delete")]
        public ActionResult DeleteBook([FromBody] DeleteBookCommand command)
        {
            bool isOk;

            try
            {
                isOk = _commandHandler.Handle(command);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Невозможно удалить запись: {ex.Message}");
                return BadRequest(GetServerErrors(ModelState));
            }

            if (!isOk)
            {
                return NotFound();
            }
            return NoContent();
        }

        private List<string> GetServerErrors(ModelStateDictionary modelstate)
        {
            List<string> errors = new List<string>();
            foreach (ModelStateEntry error in modelstate.Values)
            {
                foreach (ModelError e in error.Errors)
                {
                    errors.Add(e.ErrorMessage);
                }
            }

            return errors;
        }
    }
}
