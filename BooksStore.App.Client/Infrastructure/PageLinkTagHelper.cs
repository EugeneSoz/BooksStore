using System.Collections.Generic;
using System.Linq;
using BooksStore.Domain.Contracts.Models.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace BooksStore.App.Client.Infrastructure
{
    [HtmlTargetElement("div", Attributes = "page-model")]
    public class PageLinkTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;

        public PageLinkTagHelper(IUrlHelperFactory helperFactory)
        {
            _urlHelperFactory = helperFactory;
        }

        [ViewContext] 
        [HtmlAttributeNotBound] 
        public ViewContext ViewContext { get; set; }
        public Pagination PageModel { get; set; }
        public string PageAction { get; set; }

        [HtmlAttributeName(DictionaryAttributePrefix = "page-url-")]
        public Dictionary<string, object> PageUrlValues { get; set; }
            = new Dictionary<string, object>();

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var result = new TagBuilder("ul");
            result.AddCssClass("pagination");

            var first = CreateTag(PaginationButtonType.First, urlHelper);
            var previous = CreateTag(PaginationButtonType.Previous, urlHelper);

            result.InnerHtml.AppendHtml(first);
            result.InnerHtml.AppendHtml(previous);

            foreach (var pagesKey in PageModel.Pages)
            {
                var tag = CreateTag(pagesKey, urlHelper);
                result.InnerHtml.AppendHtml(tag);
            }

            var next = CreateTag(PaginationButtonType.Next, urlHelper);
            var last = CreateTag(PaginationButtonType.Last, urlHelper);

            result.InnerHtml.AppendHtml(next);
            result.InnerHtml.AppendHtml(last);

            output.Content.AppendHtml(result);
        }

        private TagBuilder CreateTag(PaginationButtonType buttonType, IUrlHelper urlHelper)
        {
            var pageNumber = 0;
            var content = string.Empty;
            switch (buttonType) {
                case PaginationButtonType.First:
                    pageNumber = 1;
                    content = "<i class=\"fas fa-angle-double-left\"></i>";
                    break;
                case PaginationButtonType.Last:
                    pageNumber = PageModel.TotalPages;
                    content = "<i class=\"fas fa-angle-double-right\"></i>";
                    break;
                case PaginationButtonType.Previous:
                    pageNumber = PageModel.CurrentPage - 1;
                    content = "<i class=\"fas fa-angle-left\"></i>";
                    break;
                case PaginationButtonType.Next:
                    pageNumber = PageModel.CurrentPage + 1;
                    content = "<i class=\"fas fa-angle-right\"></i>";
                    break;
            }

            var item = new TagBuilder("li");
            var link = new TagBuilder("a");

            PageUrlValues["page"] = pageNumber;
            link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            item.AddCssClass(CheckIfDisabled(buttonType) ? "page-item disabled" : "page-item");

            link.AddCssClass("page-link");

            link.InnerHtml.AppendHtml(content);
            item.InnerHtml.AppendHtml(link);

            return item;
        }

        private TagBuilder CreateTag(int pageNumber, IUrlHelper urlHelper)
        {
            var item = new TagBuilder("li");
            var link = new TagBuilder("a");
            PageUrlValues["page"] = pageNumber;
            link.Attributes["href"] = urlHelper.Action(PageAction, PageUrlValues);
            item.AddCssClass(pageNumber == PageModel.CurrentPage ? "page-item active" : "page-item");
            link.AddCssClass("page-link");
            link.InnerHtml.Append(pageNumber.ToString());
            item.InnerHtml.AppendHtml(link);
            return item;
        }

        private bool CheckIfDisabled(PaginationButtonType buttonType)
        {
            var result = false;
            switch (buttonType)
            {
                case PaginationButtonType.First:
                    result = PageModel.CurrentPage == 1;
                    break;
                case PaginationButtonType.Last:
                    result = PageModel.CurrentPage == PageModel.TotalPages;
                    break;
                case PaginationButtonType.Previous:
                    result = !PageModel.HasPreviousPage;
                    break;
                case PaginationButtonType.Next:
                    result = !PageModel.HasNextPage;
                    break;
            }
            
            return result;
        }
    }
}