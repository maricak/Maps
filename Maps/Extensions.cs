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

[AttributeUsage(AttributeTargets.Method)]
public class AjaxOnly : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext filterContext)
    {
        if (!filterContext.HttpContext.Request.IsAjaxRequest())
        {
            filterContext.HttpContext.Response.StatusCode = 404;
            filterContext.Result = new HttpNotFoundResult();
        }
        else
        {
            base.OnActionExecuting(filterContext);
        }
    }
}