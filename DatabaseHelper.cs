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
    public static OracleConnection Conn;

    public static string Host;
    public static string Port;
    public static string Sid;
    public static string User;
    public static string Password;

    public static void Set_DatabaseHelper(string host, string port, string sid, string user, string pass)
    {
        DatabaseHelper.Host = host;
        DatabaseHelper.Port = port;
        DatabaseHelper.Sid = sid;
        DatabaseHelper.User = user;
        DatabaseHelper.Password = pass;
    }

    public static bool Connect()
    {
        string connsys = "";
        try
        {
            if (User.ToUpper().Equals("SYS"))
            {
                connsys = ";DBA Privilege=SYSDBA";
            }

            string connString = "Data Source=(DESCRIPTION =(ADDRESS = (PROTOCOL = TCP)(HOST = "
                + Host + ")(PORT = " + Port + "))(CONNECT_DATA = (SERVER = DEDICATED)(SERVICE_NAME="
                + Sid + "))); User ID=" + User + " ; Password = " + Password + connsys;

            Conn = new OracleConnection();
            Conn.ConnectionString = connString;
            Conn.Open();

            return true;
        }
        catch (Exception ex)
        {
            return false;
        }
    }

    public static OracleConnection Get_Connect()
    {
        if (Conn == null)
        {
            Connect();
        }
        return Conn;
    }

    // Hàm thực thi query (non-query: insert/update/delete)
    public static int ExecuteNonQuery(string query, params OracleParameter[] parameters)
    {
        using (var conn = Get_Connect())
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
        using (var conn = Get_Connect())
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
        using (var conn = Get_Connect())
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