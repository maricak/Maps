using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
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