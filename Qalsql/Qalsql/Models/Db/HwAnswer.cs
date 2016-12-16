using System.ComponentModel.DataAnnotations.Schema;

namespace Qalsql.Models.Db
{
    public class HwAnswer
    {
        public HwAnswer()
        {
        }

        public int Id { get; set; }
        public string User { get; set; }
        public string Query { get; set; }
        public bool? Passed { get; set; }
        public string Message { get; set; }

        public int ExeId { get; set; }

        [ForeignKey("ExeId")]
        public virtual HwExercise Exercise { get; set; }

    }
}