using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Qalsql.Models
{
    public class HwExercise
    {
        public HwExercise()
        {
        }

        public int Id { get; set; }
        public int LessonId { get; set; }
        public int ExerciseNum { get; set; }
        public string Conditions { get; set; }
        public string SqlToCheck { get; set; }
    }

    public class SqlHwCheckerContext : DbContext
    {
        public DbSet<HwExercise> HwExercises { get; set; }
    }

    public static class SqlHwCheckerDbConfig
    {
        public static void Init()
        {
            using (var ctx = new SqlHwCheckerContext())
            {
                if (ctx.HwExercises.Find(1) == null)
                {
                    HwExercise[] hwExercises = new[]
                    {
                        new HwExercise
                        {
                            LessonId = 3,
                            ExerciseNum = 1,
                            Conditions =
                                "Напишите один запрос с использованием псевдонимов для таблиц и их полей, выбирающий все возможные комбинации городов (CITY) из таблиц STUDENTS, LECTURERS и UNIVERSITIES строки не должны повторяться, убедитесь в выводе только уникальных троек городов",
                            SqlToCheck =
                                "select distinct s.city as s_city, l.city as l_city, u.city as u_city from students s, lecturers l, universities u"
                        },
                        new HwExercise
                        {
                            LessonId = 3,
                            ExerciseNum = 2,
                            Conditions =
                                "Напишите запрос для вывода полей в следущем порядке: семестр, в котором он читается, идентификатора (номера ID) предмета обучения, его наименования и количества отводимых на этот предмет часов для всех строк таблицы SUBJECTS",
                            SqlToCheck = "select semester, id, name, hours from subjects"
                        },
                        new HwExercise
                        {
                            LessonId = 3,
                            ExerciseNum = 3,
                            Conditions =
                                "Выведите все строки таблицы EXAM_MARKS, в которых предмет обучения SUBJ_ID равен 4",
                            SqlToCheck = "select * from exam_marks where subj_id=4"
                        },
                        new HwExercise
                        {
                            LessonId = 3,
                            ExerciseNum = 4,
                            Conditions =
                                "Необходимо выбирать все данные, в следующем порядке Стипендия, Курс, Фамилия, Имя  из таблицы STUDENTS, причем интересуют студенты, родившиеся после '1993-07-21'",
                            SqlToCheck =
                                "select stipend, course, surname, name from students where birthday>''1993-07-21'' order by birthday"
                        },
                        new HwExercise
                        {
                            LessonId = 3,
                            ExerciseNum = 5,
                            Conditions =
                                "Вывести на экран все предметы: их наименования и кол-во часов для каждого из них в 1-м семестре и при этом кол-во часов не должно превышать 40",
                            SqlToCheck = "select name, hours from  subjects where semester=1 and hours<40"
                        },
                        // new HwExercise { LessonId  = 3, ExerciseNum = 6, Conditions = "", SqlToCheck = ""},
                    };

                    ctx.HwExercises.AddRange(hwExercises);
                    ctx.SaveChanges();
                }
            }
        }
    }
}