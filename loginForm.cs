using Oracle.ManagedDataAccess.Client;
using System;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            CenterToScreen();
        }

        private void btnLogin_Click(object sender, System.EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();

            using (var conn = DatabaseHelper.Get_Connect())
            {
                var cmd = new OracleCommand("SELECT MaNguoiDung, MatKhauHash, Salt, VaiTro, TrangThai FROM NguoiDung WHERE TenDangNhap = :u", conn);
                cmd.Parameters.Add(new OracleParameter("u", username));
                var reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    string storedHash = reader["MatKhauHash"].ToString();
                    string salt = reader["Salt"].ToString();
                    string role = reader["VaiTro"].ToString();
                    bool trangThai = Convert.ToInt32(reader["TrangThai"]) == 1;

                    if (!trangThai)
                    {
                        MessageBox.Show("Tài khoản đang bị khóa!");
                        return;
                    }

                    // Hash lại mật khẩu nhập vào với salt từ DB
                    string enteredHash = SecurityUtils.HashPassword(password, salt);

                    if (enteredHash == storedHash)
                    {
                        SessionManager.UserId = reader["MaNguoiDung"].ToString();
                        SessionManager.Username = username;
                        SessionManager.RoleCode = role;
                        SessionManager.IsLoggedIn = true;

                        this.Hide();
                        new MainForm().Show();
                    }
                    else
                    {
                        MessageBox.Show("Sai mật khẩu!");
                    }
                }
                else
                {
                    MessageBox.Show("Tài khoản không tồn tại!");
                }
            }
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            RegisterForm registerForm = new RegisterForm();
            registerForm.ShowDialog();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            DatabaseHelper.Set_DatabaseHelper(
                host: "localhost",
                port: "1521",
                sid: "orcl",
                user: "QL_DIEMRENLUYEN",
                pass: "123456"
            );
        }
    }
}
