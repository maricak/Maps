using System.Web.Mvc;

namespace Maps.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
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