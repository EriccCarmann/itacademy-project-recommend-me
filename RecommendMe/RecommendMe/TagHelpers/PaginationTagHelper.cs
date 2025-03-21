using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using RecommendMe.MVC.Models;

namespace RecommendMe.MVC.TagHelpers
{
    public class PaginationTagHelper : TagHelper
    {
        private readonly IUrlHelperFactory _urlHelperFactory;
        public PageInfo pageInfo {get; set; }
        public string pageAction { get; set; }

        //нужен контекст о странице, чтобы сделать кнопку и тд.
        [ViewContext]
        [HtmlAttributeNotBound]
        public ViewContext ViewContext { get; set; }

        public PaginationTagHelper(IUrlHelperFactory urlHelperFactory)
        {
            _urlHelperFactory = urlHelperFactory;
        }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            //не идеальная версия пагинатора
            var urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
            var result = new TagBuilder("div");

            result.AddCssClass("btn-group");

            //на лету конвертировать значения, чтобы индекс не был 0 для пользователя
            for (int i = 1; i <= pageInfo.TotalPages; i++) 
            {
                //generate links
                var itemTag = new TagBuilder("a");
                var anchorInnerHtml = i.ToString();
                itemTag.AddCssClass("btn btn-outline-primary");

                if (ViewContext.HttpContext.Request.Query.ContainsKey("pageNumber") &&
                    int.TryParse(ViewContext.HttpContext.Request.Query["pageNumber"],
                    out var actualPage))
                {
                    if (i == actualPage)
                    {
                        itemTag.AddCssClass("active");
                    }
                }
                else
                {
                    if (i == 1)
                    {
                        itemTag.AddCssClass("active");
                    }
                }

                //using Microsoft.AspNetCore.Mvc;
                itemTag.Attributes["href"] = urlHelper.Action(pageAction, new { pageNumber = i });

                itemTag.InnerHtml.AppendHtml(anchorInnerHtml);
                result.InnerHtml.AppendHtml(itemTag);
            }

            output.TagName = "div";
            //output.TagMode = TagMode.SelfClosing;
            output.Content.AppendHtml(result.InnerHtml);
        }
    }
}
