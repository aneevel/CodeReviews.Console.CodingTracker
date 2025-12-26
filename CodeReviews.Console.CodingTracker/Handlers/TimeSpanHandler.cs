namespace CodeReviews.Console.CodingTracker.Handlers
{
    public class TimeSpanHandler : SqliteTypeHandler<TimeSpan>
    {
        public override TimeSpan Parse(object value) => TimeSpan.Parse((string)value);
    }
}
