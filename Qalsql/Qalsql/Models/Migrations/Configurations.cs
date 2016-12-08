using System.Data.Entity.Migrations;
using System.Linq;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Qalsql.Models.Db;

namespace Qalsql.Models.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<QalSqlContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            ContextKey = "QalSql";
        }

        protected override void Seed(QalSqlContext context)
        {
            //if (!context.Roles.Any())
            //{
            //    RoleManager<UserRole>();
            //}

            var userAdmin = context.Users.FirstOrDefault(u => u.UserName == "admin");
            if (userAdmin == null)
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                roleManager.Create(new IdentityRole {Name = "administrators" });
                roleManager.Create(new IdentityRole {Name = "users" });

                var store = new UserStore<ApplicationUser>(context);
                var manager = new UserManager<ApplicationUser>(store);
                var user = new ApplicationUser()
                {
                    UserName = "admin",
                    Email = "admin@qalsql.ua",
                    Group = 0,
                    EmailConfirmed = true,
                    TrainingMode = 0
                };
                manager.Create(user, "Password123!");

                manager.AddToRole(user.Id, "administrators");
            }

            if (!context.HwExercises.Any())
            {
                HwExercise[] hwExercises = new[]
                {
                    new HwExercise
                    {
                        LessonId = 3,
                        ExerciseNum = 1,
                        Conditions =
                            "Напишите один запрос с использованием псевдонимов для таблиц и их полей, выбирающий все возможные комбинации городов (CITY) из таблиц STUDENTS, LECTURERS и UNIVERSITIES строки не должны повторяться, убедитесь в выводе только уникальных троек городов",
                        QueryCheck =
                            "select distinct s.city as s_city, l.city as l_city, u.city as u_city from students s, lecturers l, universities u"
                    },
                    new HwExercise
                    {
                        LessonId = 3,
                        ExerciseNum = 2,
                        Conditions =
                            "Напишите запрос для вывода полей в следущем порядке: семестр, в котором он читается, идентификатора (номера ID) предмета обучения, его наименования и количества отводимых на этот предмет часов для всех строк таблицы SUBJECTS",
                        QueryCheck = "select semester, id, name, hours from subjects"
                    },
                    new HwExercise
                    {
                        LessonId = 3,
                        ExerciseNum = 3,
                        Conditions =
                            "Выведите все строки таблицы EXAM_MARKS, в которых предмет обучения SUBJ_ID равен 4",
                        QueryCheck = "select * from exam_marks where subj_id=4"
                    },
                    new HwExercise
                    {
                        LessonId = 3,
                        ExerciseNum = 4,
                        Conditions =
                            "Необходимо выбирать все данные, в следующем порядке Стипендия, Курс, Фамилия, Имя  из таблицы STUDENTS, причем интересуют студенты, родившиеся после '1993-07-21'",
                        QueryCheck =
                            "select stipend, course, surname, name from students where birthday>''1993-07-21'' order by birthday"
                    },
                    new HwExercise
                    {
                        LessonId = 3,
                        ExerciseNum = 5,
                        Conditions =
                            "Вывести на экран все предметы: их наименования и кол-во часов для каждого из них в 1-м семестре и при этом кол-во часов не должно превышать 40",
                        QueryCheck = "select name, hours from  subjects where semester=1 and hours<40"
                    },
                    // new HwExercise { LessonId  = 3, ExerciseNum = 6, Conditions = "", QueryCheck = ""},
                };

                context.HwExercises.AddRange(hwExercises);

                context.SaveChanges();
            }
        }
    }
}