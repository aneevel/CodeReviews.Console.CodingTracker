namespace CodeReviews.Console.CodingTracker.aneevel.Handlers
{
    public class TimeSpanHandler : SqliteTypeHandler<TimeSpan>
    {
        public override TimeSpan Parse(object value) => TimeSpan.Parse((string)value);
    }
}
