using System.Web.Mvc;

namespace Maps.Controllers
{
    public class HomeController : Controller
    {
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                logger.InfoFormat("UserId={0}", User.Identity.GetUser().Id);

                return RedirectToAction("Index", "Map");
            }   
            else
            {
                logger.InfoFormat("Not authenticated");

                return RedirectToAction("Login", "Account");
            }
        }

        public ActionResult Error()
        {
            return View();
        }
        public ActionResult NotFound()
        {
            return View();
        }
        public ActionResult Forbidden()
        {
            return View();
        }

        public ActionResult ServerError()
        {
            return View();
        }
    }
}