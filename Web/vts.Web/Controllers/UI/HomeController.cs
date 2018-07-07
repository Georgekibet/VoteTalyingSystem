using System.Web.Mvc;
using vts.Web.Helpers;

namespace vts.Web.Controllers.UI
{
    [Authorize]
    [AccessDeniedAuthorize(Roles = "Admin")]

    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}