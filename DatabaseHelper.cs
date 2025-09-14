using System.Data;
using Oracle.ManagedDataAccess.Client;

public class DatabaseHelper
{
    private static string connectionString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = localhost)(PORT = 1521))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME= orcl))); User ID=sys ;Password = 123; DBA Privilege=SYSDBA";

    public static OracleConnection GetConnection()
    {
        return new OracleConnection(connectionString);
    }

    // Hàm thực thi query (non-query: insert/update/delete)
    public static int ExecuteNonQuery(string query, params OracleParameter[] parameters)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = new OracleCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteNonQuery();
            }
        }
    }

    // Hàm lấy dữ liệu (scalar: ví dụ đếm hoặc lấy giá trị đơn)
    public static object ExecuteScalar(string query, params OracleParameter[] parameters)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = new OracleCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                return cmd.ExecuteScalar();
            }
        }
    }

    // Hàm lấy DataTable (cho select nhiều dòng)
    public static DataTable ExecuteQuery(string query, params OracleParameter[] parameters)
    {
        using (var conn = GetConnection())
        {
            conn.Open();
            using (var cmd = new OracleCommand(query, conn))
            {
                cmd.Parameters.AddRange(parameters);
                using (var adapter = new OracleDataAdapter(cmd))
                {
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
        }
    }
}