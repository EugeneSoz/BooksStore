using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksStore.App.Contracts.Command;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Command;
using BooksStore.App.Handlers.Query;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Domain.Contracts.Models.Publishers;
using BooksStore.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using OnlineBooksStore.Domain.Contracts.Models.Publishers;

namespace BooksStore.App.Client.Controllers
{
    public class PublishersController : Controller
    {
        private readonly PublisherCommandHandler _commandHandler;
        private readonly PublisherQueryHandler _queryHandler;

        public PublishersController(PublisherCommandHandler commandHandler, 
            PublisherQueryHandler queryHandler)
        {
            _commandHandler = commandHandler;
            _queryHandler = queryHandler;
        }

        public IActionResult Index()
        {
            return View();
        }

                [HttpGet("publisher/{id}")]
        public Publisher GetPublisher([FromRoute] PublisherIdQuery query)
        {
            try
            {
                var publiser = _queryHandler.Handle(query);

                return publiser;
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        [HttpPost("publishers")]
        public PagedResponse<PublisherResponse> GetPublishers([FromBody] PageFilterQuery query)
        {
            try
            {
                return _queryHandler.Handle(query);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }

        [HttpPost("publishersforselection")]
        public List<PublisherResponse> GetPublishersForSelection([FromBody] SearchTermQuery query)
        {
            return _queryHandler.Handle(query);
        }

        [HttpPost("create")]
        public ActionResult CreatePublisher([FromBody] CreatePublisherCommand command)
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
