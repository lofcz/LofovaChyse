using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Routing;

namespace LofovaChyse.Class
{
    public static class ImageActionLinkHelper
    {
        public static IHtmlString ImageActionLink(this AjaxHelper helper, string imageUrl, string altText, string actionName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes = null)
        {
            var builder = new TagBuilder("img");
            builder.MergeAttribute("src", imageUrl);
            builder.MergeAttribute("alt", altText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            var link = helper.ActionLink("[replaceme]", actionName, routeValues, ajaxOptions).ToHtmlString();
            return MvcHtmlString.Create(link.Replace("[replaceme]", builder.ToString(TagRenderMode.SelfClosing)));
        }

        public static MvcHtmlString RawActionLink(this AjaxHelper ajaxHelper, string linkText, string actionName, string controllerName, object routeValues, AjaxOptions ajaxOptions, object htmlAttributes)
        {
            var repID = Guid.NewGuid().ToString();
            var lnk = ajaxHelper.ActionLink(repID, actionName, controllerName, routeValues, ajaxOptions, htmlAttributes);
            return MvcHtmlString.Create(lnk.ToString().Replace(repID, linkText));
        }
    }
}