using System.Collections.Generic;

namespace Qalsql.Models.Db
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
        public string QueryCheck { get; set; }

        public virtual ICollection<HwAnswer> HwAnswers { get; set; } = new HashSet<HwAnswer>();
    }
}