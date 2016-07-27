using System;
using System.Linq;
using System.Web.Mvc;
using Qalsql.Models;

namespace Qalsql.Controllers
{
    public class SqlCheckerController : Controller
    {
        private SqlHwCheckerContext _db = new SqlHwCheckerContext();

        // GET: SqlChecker
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListOfTasks(int lessonId = 3)
        {
            return View(_db.HwExercises.Where(x => x.LessonId == lessonId).ToList());
        }

        [HttpPost]
        public ActionResult SendSql(string sql)
        {
            SqlResult sqlResult;
            if (ModelState.IsValid)
            {
                sqlResult = SqlExecutor.SendQuery(sql);
            }
            else
            {
                sqlResult = CreatorSqlResult.Fail(sql,
                    ModelState.Values
                        .SelectMany(m => m.Errors, (parent, child) => child.ErrorMessage)
                        .Aggregate((s, n) => s + ";" + n));
            }
            return View(sqlResult);
        }

        public ActionResult CheckSql()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CheckSql(int lessonId, int taskId, string sql)
        {
            string mainSql = SqlTasks.Get(lessonId, taskId);

            string combainedSql = String.Format("{0} except {1} union all {1} except {0}", sql, mainSql);

            SqlResult checkResult = SqlExecutor.SendQuery(combainedSql);
            SqlResult testingSql = SqlExecutor.SendQuery(sql);

            if (checkResult.Status.IsOk)
            {
                return View("CheckSqlOk");
            }

            SqlResult etalonResult = SqlExecutor.SendQuery(mainSql);

            return View("CheckSqlFail");
        }
    }

    public static class SqlTasks
    {
        public static string Get(int lessonId, int taskId)
        {
            return "select top(1) * from Students";
        }
    }
}