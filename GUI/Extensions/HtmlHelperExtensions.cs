using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Shared.Models;
using System;

namespace GUI.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static IHtmlContent RenderPagination<T>(this IHtmlHelper _, PaginatedList<T> paginatedList,
            string keyword,
            Func<string, int, int, string> paginationLinkGenerator)
        {
            if (paginatedList == null)
                throw new ArgumentNullException(nameof(paginatedList));
            if (paginatedList.IsEmpty)
                return new HtmlString("");

            var navTag = new TagBuilder("nav");
            navTag.Attributes.Add("aria-label", "Page navigation example");

            var ulTag = new TagBuilder("ul");
            ulTag.AddCssClass("pagination");
            ulTag.AddCssClass("justify-content-center");

            var previousLiTag = new TagBuilder("li");
            previousLiTag.AddCssClass("page-item");

            var previousATag = new TagBuilder("a");
            previousATag.AddCssClass("page-link");
            previousATag.Attributes.Add("tabindex", "-1");
            if (paginatedList.HasPreviousPage)
                previousATag.Attributes.Add("href", paginationLinkGenerator.Invoke(keyword, paginatedList.PageNumber - 1, paginatedList.PageSize));
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
                    aTag.Attributes.Add("href", paginationLinkGenerator.Invoke(keyword, i, paginatedList.PageSize));
                liTag.InnerHtml.AppendHtml(aTag);
                ulTag.InnerHtml.AppendHtml(liTag);
            }

            var nextLiTag = new TagBuilder("li");
            nextLiTag.AddCssClass("page-item");

            var nextATag = new TagBuilder("a");
            nextATag.AddCssClass("page-link");
            nextATag.Attributes.Add("tabindex", "-1");
            if (paginatedList.HasNextPage)
                nextATag.Attributes.Add("href", paginationLinkGenerator.Invoke(keyword, paginatedList.PageNumber + 1, paginatedList.PageSize));
            nextATag.InnerHtml.Append("Next");

            nextLiTag.InnerHtml.AppendHtml(nextATag);

            ulTag.InnerHtml.AppendHtml(nextLiTag);

            navTag.InnerHtml.AppendHtml(ulTag);

            return navTag;
        }

        public static string ProductImageUrl(this IUrlHelper _, string imageUrl)
        {
            return $"https://ec2-13-215-160-187.ap-southeast-1.compute.amazonaws.com:3000/products/images/{imageUrl}";
        }

        public static string ShopImageUrl(this IUrlHelper _, string imageUrl)
        {
            return $"https://ec2-13-215-160-187.ap-southeast-1.compute.amazonaws.com:3000/interfaces/images/{imageUrl}";
        }
    }
}