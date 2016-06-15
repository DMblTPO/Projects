namespace Qalsql.Models
{
    public class SqlRequest
    {
        public int Id { get; set; }
        public string SqlText { get; set; }
        public int Lesson { get; set; }
        public int TaskNumber { get; set; }
    }
}