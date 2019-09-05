using System.Web.Mvc;

namespace Maps.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Map");
            }   
            else
            {
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