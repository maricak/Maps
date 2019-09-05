using Microsoft.AspNet.Identity;
using System.Collections.Generic;

namespace Maps.Entities
{
    public class ManageIndexViewModel
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public bool BrowserRemembered { get; set; }
    }
}
