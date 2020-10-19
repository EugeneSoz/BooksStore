using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BooksStore.App.Contracts.Command;
using BooksStore.App.Contracts.Query;
using BooksStore.App.Handlers.Command;
using BooksStore.App.Handlers.Query;
using BooksStore.Domain.Contracts.Models;
using BooksStore.Domain.Contracts.Models.Categories;
using BooksStore.Domain.Contracts.Models.Pages;
using BooksStore.Persistence.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BooksStore.App.Client.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryQueryHandler _queryHandler;
        private readonly CategoryCommandHandler _commandHandler;
        private readonly IUrlHelper _urlHelper;

        public CategoriesController(
            CategoryQueryHandler queryHandler, 
            CategoryCommandHandler commandHandler, 
            IUrlHelperFactory urlHelperFactory,
            IActionContextAccessor actionContextAccessor)
        {
            _queryHandler = queryHandler ?? throw new ArgumentNullException(nameof(queryHandler));
            _commandHandler = commandHandler ?? throw new ArgumentNullException(nameof(commandHandler));
            _urlHelper = urlHelperFactory.GetUrlHelper(actionContextAccessor.ActionContext);
        }

        [HttpGet]
        [HttpPost]
        public IActionResult ShowCategories(AdminFilter adminFilter, PageOptions pageOptions)
        {
            var query = new PageConditionsQuery(adminFilter, pageOptions);

            var model = _queryHandler.Handle(query);
            model.ToolbarViewModel.FormUrl = _urlHelper.Action("CreateCategory", "Categories", new { Id = 0 });
            
            return View("CategoriesSection", model);
        }

        [HttpGet("category/{id}")]
        public Category GetCategory([FromRoute] CategoryIdQuery query)
        {
            return _queryHandler.Handle(query);
        }
        
        [HttpGet("parentcategories")]
        public List<Category> GetParentCategories(ParentCategoryCategoryQuery query)
        {
            return _queryHandler.Handle(query);
        }

        [HttpPost("categoriesforselection")]
        public List<CategoryResponse> GetCategoriesForSelection([FromBody] SearchTermQuery query)
        {
            return _queryHandler.Handle(query);
        }

        [HttpPost]
        public IActionResult CreateCategory([FromBody] CreateCategoryCommand command)
        {
            if (!ModelState.IsValid)
            {
                return Ok(GetServerErrors(ModelState));
            }

            CategoryEntity category;
            try
            {
                category = _commandHandler.Handle(command);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, $@"Невозможно создать запись: {ex.Message}");
                return BadRequest(GetServerErrors(ModelState));
            }

            return Created("", category);
        }

        [HttpPut("update")]
        public ActionResult UpdateCategory([FromBody] UpdateCategoryCommand command)
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
        public ActionResult DeleteCategory([FromBody] DeleteCategoryCommand command)
        {
            //если у категории есть дочерние, тогда удалить их
            bool isOk = _commandHandler.Handle(command);
            //в случае успеха - удалить саму родительскую категорию
            if (isOk)
            {
                return Delete(command, _commandHandler.Handle);
            }
            //удалить любую категорию, у которой нет дочерних
            return Delete(command, _commandHandler.Handle);
        }

        private ActionResult Delete(DeleteCategoryCommand command, Func<DeleteCategoryCommand, bool> commandHandler)
        {
            bool isOk;

            try
            {
                isOk = commandHandler.Invoke(command);
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
