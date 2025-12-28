using Microsoft.Data.SqlClient;
using System.Data;

namespace QL_DienTuGiaDung.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("DefaultConnection not found in configuration");
        }

        public List<Dictionary<string, object?>> ExecuteQuery(string sqlQuery, CommandType type = CommandType.Text, SqlParameter[]? parameters = null)
        {
            var result = new List<Dictionary<string, object?>>();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.CommandType = type;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            connection.Open();

            using SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                var row = new Dictionary<string, object?>();

                for (int i = 0; i < reader.FieldCount; i++)
                {
                    row[reader.GetName(i)] = reader.IsDBNull(i) ? null : reader.GetValue(i);
                }

                result.Add(row);
            }

            return result;
        }

        public int ExecuteNonQuery(string sql, CommandType type = CommandType.Text, SqlParameter[]? parameters = null)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection)
            {
                CommandType = type
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            connection.Open();
            return command.ExecuteNonQuery();
        }

        public object? ExecuteScalar(string sql, CommandType type = CommandType.Text, SqlParameter[]? parameters = null)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection)
            {
                CommandType = type
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            connection.Open();
            return command.ExecuteScalar();
        }
    }
}
