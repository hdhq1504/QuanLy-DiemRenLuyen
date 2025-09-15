using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;
using Oracle.ManagedDataAccess.Client;

public class DatabaseHelper
{
    private static OracleConnection Conn;

    private static string Host;
    private static string Port;
    private static string Sid;
    private static string User;
    private static string Password;

    public static void Set_DatabaseHelper(string host, string port, string sid, string user, string pass)
    {
        Host = host;
        Port = port;
        Sid = sid;
        User = user;
        Password = pass;
    }

    public static bool Connect()
    {
        try
        {
            string connsys = "";
            if (!string.IsNullOrEmpty(User) && User.ToUpper().Equals("SYS"))
            {
                connsys = ";DBA Privilege=SYSDBA";
            }

            // Dùng SID (orcl)
            string connString =
                "Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=" + Host +
                ")(PORT=" + Port + "))(CONNECT_DATA=(SID=" + Sid + ")));" +
                "User Id=" + User + ";Password=" + Password + connsys;

            Conn = new OracleConnection(connString);
            Conn.Open();
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("Lỗi kết nối Oracle: " + ex.Message);
            Conn = null;
            return false;
        }
    }

    public static OracleConnection Get_Connect()
    {
        if (Conn == null || Conn.State != ConnectionState.Open)
        {
            bool ok = Connect();
            if (!ok)
            {
                throw new Exception("Không thể kết nối tới Oracle DB. Vui lòng kiểm tra cấu hình!");
            }
        }
        return Conn;
    }

    // Hàm thực thi query (non-query: insert/update/delete)
    public static int ExecuteNonQuery(string query, params OracleParameter[] parameters)
    {
        using (var conn = Get_Connect())
        using (var cmd = new OracleCommand(query, conn))
        {
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteNonQuery();
        }
    }

    // Hàm lấy dữ liệu (scalar: ví dụ đếm hoặc lấy giá trị đơn)
    public static object ExecuteScalar(string query, params OracleParameter[] parameters)
    {
        using (var conn = Get_Connect())
        using (var cmd = new OracleCommand(query, conn))
        {
            if (parameters != null)
                cmd.Parameters.AddRange(parameters);
            return cmd.ExecuteScalar();
        }
    }

    // Hàm lấy DataTable (cho select nhiều dòng)
    public static DataTable ExecuteQuery(string query, params OracleParameter[] parameters)
    {
        using (var conn = Get_Connect())
        using (var cmd = new OracleCommand(query, conn))
        {
            if (parameters != null)
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