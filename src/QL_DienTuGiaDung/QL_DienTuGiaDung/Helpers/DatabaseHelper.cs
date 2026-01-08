using System.Data;
using Microsoft.Data.SqlClient;

namespace QL_DienTuGiaDung.Helpers
{
    public class DatabaseHelper
    {
        private readonly string _connectionString;

        public DatabaseHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new ArgumentNullException("DefaultConnection not found in configuration");
        }

        public DataTable ExecuteDataTable(
            string sqlQuery,
            CommandType type = CommandType.Text,
            SqlParameter[]? parameters = null)
        {
            DataTable dataTable = new DataTable();

            using SqlConnection connection = new SqlConnection(_connectionString);
            using SqlCommand command = new SqlCommand(sqlQuery, connection);
            command.CommandType = type;

            if (parameters != null)
            {
                command.Parameters.AddRange(parameters);
            }

            using SqlDataAdapter adapter = new SqlDataAdapter(command);
            adapter.Fill(dataTable);

            return dataTable;
        }

        public int ExecuteNonQuery(
            string sql,
            CommandType type = CommandType.Text,
            SqlParameter[]? parameters = null)
        {
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(sql, connection)
            {
                CommandType = type
            };

            if (parameters != null)
                command.Parameters.AddRange(parameters);

            SqlParameter? returnParam = null;

            if (type == CommandType.StoredProcedure)
            {
                returnParam = command.Parameters.Add(
                    "@ReturnValue",
                    SqlDbType.Int
                );
                returnParam.Direction = ParameterDirection.ReturnValue;
            }

            connection.Open();
            int rowsAffected = command.ExecuteNonQuery();

            if (returnParam != null && returnParam.Value != DBNull.Value)
                return (int)returnParam.Value;

            return rowsAffected;
        }

        public object? ExecuteScalar(
            string sql,
            CommandType type = CommandType.Text,
            SqlParameter[]? parameters = null)
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
