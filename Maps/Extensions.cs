using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using System.Web.Mvc.Html;
using Maps.Data;
using Maps.Entities;
using Microsoft.AspNet.Identity;

public static class Extensions
{
    public static User GetUser(this System.Security.Principal.IIdentity identity)
    {
        if (identity.IsAuthenticated)
        {
            using (var db = new MapsDbContext())
            {
                return db.Users.Find(identity.GetUserId());
            }
        }
        else
        {
            return null;
        }
    }
}


public static class HtmlHelperExtensions
{

    public static MvcHtmlString IconActionLink(this AjaxHelper ajaxHelper, string icon, string action, object routeValues, AjaxOptions ajaxOptions)
    {
        string holder = Guid.NewGuid().ToString();
        string anchor = ajaxHelper.ActionLink(holder, action, routeValues, ajaxOptions).ToString();
        var innerHtml = "<i class=\"fas fa-" + icon + "\"></i>";
        return MvcHtmlString.Create(anchor.Replace(holder, innerHtml));
    }

    public static MvcHtmlString IconActionLink(this AjaxHelper ajaxHelper, string icon, string action, object routeValues, AjaxOptions ajaxOptions, IDictionary<String, Object> htmlAttributes)
    {
        string holder = Guid.NewGuid().ToString();
        string anchor = ajaxHelper.ActionLink(holder, action, routeValues, ajaxOptions, htmlAttributes).ToString();
        var innerHtml = "<i class=\"fas fa-" + icon + "\"></i>";
        return MvcHtmlString.Create(anchor.Replace(holder, innerHtml));
    }

    public static MvcHtmlString IconActionLink(this HtmlHelper htmlHelper, string icon, string action, object routeValues)
    {
        string holder = Guid.NewGuid().ToString();
        string anchor = htmlHelper.ActionLink(holder, action, routeValues).ToString();
        var innerHtml = "<i class=\"fas fa-" + icon + "\"></i>";
        return MvcHtmlString.Create(anchor.Replace(holder, innerHtml));
    }

}