namespace CodeReviews.Console.CodingTracker
{
    internal class AppSettings(string connectionString)
    {
        private readonly string _connectionString = connectionString;
        public string ConnectionString
        {
            get { return _connectionString; }
        }
    }
}
