namespace LadeBrik.Models
{
    public class LadeBrikArchiveModel : LadeBrikBase
    {
        public DateTime ChangedAt { get; set; }
        public Operation Operation { get; set; }
    }
    public enum Operation
    {
        Create,
        Update,
        Delete
    }
}
