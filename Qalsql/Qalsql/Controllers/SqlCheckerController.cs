using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Qalsql.Models;
using Qalsql.Models.Db;

namespace Qalsql.Controllers
{
    [Authorize]
    public class SqlCheckerController : AuthorizedController
    {
        private readonly QalSqlContext _db = new QalSqlContext();

        // GET: SqlChecker
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> ListOfTasks(int lessonId = 3, int activeTask = 1)
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
                            Passed = a.Passed.HasValue && a.Passed.Value,
                            Message = a.Message
                        }
                );

            ViewBag.ActiveTask = activeTask;

            return View(await query.ToListAsync());
        }

        [HttpPost]
        public async Task<ActionResult> SendSql(string sql)
        {
            SqlResult sqlResult;
            if (ModelState.IsValid)
            {
                sqlResult = await SqlExecutor.SendQueryAsync(sql);
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
        public async Task<ActionResult> CheckSql(int lessonId, int taskId, string sql)
        {
            var exercise = await _db.HwExercises.FirstOrDefaultAsync(t => t.LessonId == lessonId && t.ExerciseNum == taskId);

            if (exercise == null)
            {
                throw new Exception("there is no task in db");
            }

            var sqlResult = await SqlExecutor.CheckQuery(sql, exercise.QueryCheck);

            var answer = _db.HwAnswers.FirstOrDefault(x => x.ExeId == exercise.Id && x.User == UserId);

            if (answer == null)
            {
                _db.HwAnswers.Add(new HwAnswer
                {
                    ExeId = exercise.Id,
                    Passed = sqlResult.Status.Success,
                    Query = sql,
                    User = UserId,
                    Message = sqlResult.Status.Message
                });
            }
            else
            {
                var ok = sqlResult.Status.Success;
                answer.Query = sql;
                answer.Passed = ok;
                answer.Message = !ok? sqlResult.Status.Message: null;
            }

            _db.SaveChanges();

            return RedirectToAction("ListOfTasks", new {lessonId, activeTask = taskId});
        }
    }
}