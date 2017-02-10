using System.Web.Mvc;

namespace Qalsql.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Faq()
        {
            ViewBag.Message = "FAQ в разработке.";

            return View();
        }

        public ActionResult Contact()
        {
            var user = User.Identity.Name;
            ViewBag.Message = $"Привет, {user}! Как со мной связаться?";

            return View();
        }

        public ActionResult ListOfTasks()
        {
            throw new System.NotImplementedException();
        }
    }
}