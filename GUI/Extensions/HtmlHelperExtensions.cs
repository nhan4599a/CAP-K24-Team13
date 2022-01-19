using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Models;
using System;

namespace GUI.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent RenderPagination<T>(this IHtmlHelper _, PaginatedList<T> paginatedList,
            string keyword, int pageSize,
            Func<string, int, int, string> paginationLinkGenerator)
        {
            if (paginatedList == null)
                throw new ArgumentNullException(nameof(paginatedList));
            if (paginatedList.IsEmpty)
                return new HtmlString("");

            var navTag = new TagBuilder("nav");
            navTag.Attributes.Add("style", "text-align: center;");

            var ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination");
            ulTag.AddCssClass("justify-content-center");

            var previousLiTag = new TagBuilder("li");
            previousLiTag.AddCssClass("page-item");
            if (!paginatedList.HasPreviousPage)
                previousLiTag.AddCssClass("disabled");

            var previousATag = new TagBuilder("a");
            previousATag.AddCssClass("page-link");
            previousATag.Attributes.Add("tabindex", "-1");
            if (paginatedList.HasPreviousPage)
                previousATag.Attributes.Add("href", paginationLinkGenerator.Invoke(keyword, paginatedList.PageNumber - 1, pageSize));
            previousATag.InnerHtml.Append("Previous");

            previousLiTag.InnerHtml.AppendHtml(previousATag);

            ulTag.InnerHtml.AppendHtml(previousLiTag);

            for (int i = 1; i <= paginatedList.MaxPageNumber; i++)
            {
                var liTag = new TagBuilder("li");
                var aTag = new TagBuilder("a");
                liTag.AddCssClass("page-item");
                aTag.AddCssClass("page-link");
                aTag.InnerHtml.Append(i.ToString());
                if (i != paginatedList.PageNumber)
                    aTag.Attributes.Add("href", paginationLinkGenerator.Invoke(keyword, i, pageSize));
                liTag.InnerHtml.AppendHtml(aTag);
                ulTag.InnerHtml.AppendHtml(liTag);
            }

            var nextLiTag = new TagBuilder("li");
            nextLiTag.AddCssClass("page-item");
            if (!paginatedList.HasPreviousPage)
                nextLiTag.AddCssClass("disabled");

            var nextATag = new TagBuilder("a");
            nextATag.AddCssClass("page-link");
            nextATag.Attributes.Add("tabindex", "-1");
            if (!paginatedList.HasNextPage)
                nextATag.Attributes.Add("aria-disabled", "true");
            else
                nextATag.Attributes.Add("href", paginationLinkGenerator.Invoke(keyword, paginatedList.PageNumber + 1, pageSize));
            nextATag.InnerHtml.Append("Next");

            nextLiTag.InnerHtml.AppendHtml(nextATag);

            ulTag.InnerHtml.AppendHtml(nextLiTag);

            navTag.InnerHtml.AppendHtml(ulTag);

            return navTag;
        }
    }
}
