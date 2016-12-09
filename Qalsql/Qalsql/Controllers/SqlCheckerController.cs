using System;
using System.Linq;
using System.Web.Mvc;
using Qalsql.Models;
using Qalsql.Models.Db;

namespace Qalsql.Controllers
{
    [Authorize]
    public class SqlCheckerController : AuthorizedController
    {
        private QalSqlContext _db = new QalSqlContext();

        // GET: SqlChecker
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListOfTasks(int lessonId = 3, int activeTask = 1)
        {
            var answers = _db.HwAnswers.Where(x => x.User == UserId);
            var exercises = _db.HwExercises.Where(x => x.LessonId == lessonId);

            var query = exercises.GroupJoin(
                    answers,
                    e => e.Id,
                    a => a.ExeId,
                    (e, a) => new {Exe = e, Ans = a})
                .SelectMany(
                    e => e.Ans.DefaultIfEmpty(),
                    (e, a) =>
                        new TaskDto
                        {
                            Exercise = e.Exe,
                            Answer = a.Query,
                            Passed = a.Passed.HasValue && a.Passed.Value
                        }
                );

            ViewBag.ActiveTask = activeTask;

            return View(query.ToList());
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

            var answer = _db.HwAnswers.FirstOrDefault(x => x.ExeId == exercise.Id && x.User == UserId);

            if (answer == null)
            {
                _db.HwAnswers.Add(new HwAnswer
                {
                    ExeId = exercise.Id,
                    Passed = checkResult.Status.IsOk,
                    Query = sql,
                    User = UserId
                });
            }
            else
            {
                if (!(answer.Passed.HasValue && answer.Passed.Value))
                {
                    answer.Query = sql;
                }
                answer.Passed = checkResult.Status.IsOk;
            }

            _db.SaveChanges();

            return RedirectToAction("ListOfTasks", new {lessonId, activeTask = taskId});
        }
    }
}