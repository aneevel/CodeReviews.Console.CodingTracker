namespace CodeReviews.Console.CodingTracker
{
    internal class AppSettings(string path, string name, string connectionString)
    {
        private readonly string _path = path;
        public string Path
        {
            get { return _path; }
        }
        private readonly string _name = name;
        public string Name
        {
            get { return _name; }
        }
        private readonly string _connectionString = connectionString;
        public string ConnectionString
        {
            get { return _connectionString; }
        }
    }
}
