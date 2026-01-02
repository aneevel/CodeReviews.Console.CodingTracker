using System.Data.SQLite;
using CodeReviews.Console.CodingTracker.aneevel.Handlers;
using Dapper;
using Serilog;

namespace CodeReviews.Console.CodingTracker.aneevel.Database
{
    internal class SqliteDatabaseInitializer : IDatabaseInitializer
    {
        private readonly string _connectionString;

        public SqliteDatabaseInitializer(string connectionString)
        {
            _connectionString = connectionString;
            
            if (InitializeDatabase() == -1)
            {
                Environment.Exit(-1);
            }

            SqlMapper.AddTypeHandler(new TimeSpanHandler());
        }

        public int InitializeDatabase()
        {
            return CreateTable();
        }

        private int CreateTable()
        {
            using var db = new SQLiteConnection(_connectionString);
            const string sql = @"CREATE TABLE IF NOT EXISTS CodingSessions (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT, 
                    StartTime VARCHAR(255) NOT NULL, 
                    EndTime VARCHAR(255) NOT NULL, 
                    Duration VARCHAR(255) NOT NULL
                );";

            try
            {
                db.Execute(sql);
            }
            catch (SQLiteException ex)
            {
                Log.Logger.Fatal(
                    ex,
                    """
                                  Class:{Type} Method: {CreateTable}\n
                                                          Message: There was an issue creating the database!
                                  """, typeof(SqliteDatabaseInitializer), nameof(CreateTable)
                );
                return -1;
            }
            catch (Exception ex)
            {
                Log.Logger.Error(
                    ex,
                    """
                                  Class:{Type} Method: {CreateTable}\n
                                                          Message: There was an explained issue!
                                  """, typeof(SqliteDatabaseInitializer), nameof(CreateTable)
                );
                return -1;
            }
            return 0;
        }
    }
}
