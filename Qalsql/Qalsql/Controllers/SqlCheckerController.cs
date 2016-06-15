using System.Web.Mvc;
using Qalsql.Models;

namespace Qalsql.Controllers
{
    public class SqlCheckerController : Controller
    {
        // GET: SqlChecker
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SendSql(string sql)
        {
            var model = SqlExecutor.SendQuery(sql);
            return View(model);
        }
    }
}