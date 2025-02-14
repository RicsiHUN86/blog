using MySql.Data.MySqlClient;

namespace blogg
{
    public class Connect
    {
        private MySqlConnection connection;
        private string connectionString;

        public Connect()
        {
            string host = "localhost";
            string database = "Blog";
            string user = "root";
            string password = "";

            connectionString = $"SERVER={host};DATABASE={database};UID={user};PASSWORD={password};SslMode=None";
            connection = new MySqlConnection(connectionString);
        }

        public void OpenConnection()
        {
            if (connection.State == System.Data.ConnectionState.Closed)
            {
                connection.Open();
            }
        }

        public void CloseConnection()
        {
            if (connection.State == System.Data.ConnectionState.Open)
            {
                connection.Close();
            }
        }

        public MySqlConnection GetConnection()
        {
            return connection;
        }
    }
}