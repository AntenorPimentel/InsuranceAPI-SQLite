namespace Insurance.Data
{
    public class SqliteConfiguration
    {
        public string DBConnectionString { get; }

        public SqliteConfiguration(string dbConnectionString)
        {
            DBConnectionString = dbConnectionString;
        }
    }
}