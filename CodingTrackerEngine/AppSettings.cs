using System;
using System.Collections.Generic;
using System.Text;

namespace CodingTrackerEngine
{
    internal class AppSettings
    {
        private readonly string _path;
        public string Path
        {
            get { return _path; }
        }
        private readonly string _name;
        public string Name
        {
            get { return _name; }
        }
        private readonly string _connectionString;
        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public AppSettings(string path, string name, string connectionString)
        {
            this._path = path;
            this._name = name;
            this._connectionString = connectionString;
        }
    }
}
