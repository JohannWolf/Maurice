using MySql.Data.MySqlClient;

namespace Maurice.Data.Repositories
{
    public class MauriceRepository
    {
        private readonly string _connectionString;

        public MauriceRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void FetchData()
        {
            using var connection = new MySqlConnection(_connectionString);
            connection.Open();
            // Example Query
            var command = new MySqlCommand("SELECT * FROM YourTable", connection);
            var reader = command.ExecuteReader();
            while (reader.Read())
            {
                // Process data
            }
        }
    }
}
