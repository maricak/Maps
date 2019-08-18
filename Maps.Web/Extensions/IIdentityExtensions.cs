using Maps.Data;
using Maps.Entities;
using Microsoft.AspNet.Identity;

namespace Maps
{
    public static class IIdentityExtensions
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
}