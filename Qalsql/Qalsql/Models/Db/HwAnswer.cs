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

        public virtual HwExercise Exercise { get; set; }
    }
}