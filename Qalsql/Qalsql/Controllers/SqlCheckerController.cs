using System;
using System.Linq;
using System.Web.Mvc;
using Qalsql.Models;
using Qalsql.Models.Db;

namespace Qalsql.Controllers
{
    [Authorize]
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
            var exercise = _db.HwExercises.FirstOrDefault(t => t.LessonId == lessonId && t.ExerciseNum == taskId);

            if (exercise == null)
            {
                throw new Exception("there is no task in db");
            }

            string mainSql = exercise.QueryCheck;

            string combainedSql = $"{sql} except {mainSql} union all {mainSql} except {sql}";

            SqlResult checkResult = SqlExecutor.SendQuery(combainedSql);
            SqlResult testingSql = SqlExecutor.SendQuery(sql);

            if (checkResult.Status.IsOk)
            {
                ViewBag.isTaskOk = "Task is Ok!";
                return RedirectToAction("ListOfTasks");
                // return View("CheckSqlOk");
            }

            SqlResult etalonResult = SqlExecutor.SendQuery(mainSql);

            return View("CheckSqlFail");
        }
    }
}