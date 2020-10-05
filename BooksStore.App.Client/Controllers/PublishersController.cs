using System;
using System.Collections.Generic;
using BooksStore.App.Client.Filters;
using BooksStore.App.Contracts.Command;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Command;
using BooksStore.App.Handlers.Query;
using BooksStore.Domain.Contracts.Models.Publishers;
using BooksStore.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BooksStore.App.Client.Controllers
{
    [GeneralException]
    public class PublishersController : Controller
    {
        private readonly PublisherCommandHandler _commandHandler;
        private readonly PublisherQueryHandler _queryHandler;
        private readonly IUrlHelper _urlHelper;

        public PublishersController(PublisherCommandHandler commandHandler, 
            PublisherQueryHandler queryHandler, IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        public IActionResult ShowPublishers(int page)
        {

            var query = new PageFilterQuery()
            {
                CurrentPage = page == 0 ? 1 : page,
                PageSize = 20,
            };

            var model = _queryHandler.Handle(query);
            model.ToolbarViewModel.FormUrl = _urlHelper.Action("CreatePublisher", "Publishers", new { Id = 0 });
            
            return View("PublishersSection", model);
        }

        [HttpGet]
        public IActionResult CreatePublisher([FromRoute] PublisherIdQuery query)
        {
            try
            {
                var publiser = _queryHandler.Handle(query);

                return View("PublisherForm", publiser);
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        [HttpPost]
        public IActionResult CreatePublisher([FromBody] CreatePublisherCommand command)
        {
            if (!ModelState.IsValid)
            {
                return Ok(GetServerErrors(ModelState));
            }

            PublisherEntity publisher;
            try
            {
                publisher = _commandHandler.Handle(command);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Невозможно создать запись: {ex.Message}");
                return BadRequest(GetServerErrors(ModelState));
            }

            return Created("", publisher);
        }

        [HttpPost("publishersforselection")]
        public List<PublisherResponse> GetPublishersForSelection([FromBody] SearchTermQuery query)
        {
            return _queryHandler.Handle(query);
        }

        

        [HttpPut("update")]
        public ActionResult UpdatePublisher([FromBody] UpdatePublisherCommand command)
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
        public ActionResult DeletePublisher([FromBody] DeletePublisherCommand command)
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
