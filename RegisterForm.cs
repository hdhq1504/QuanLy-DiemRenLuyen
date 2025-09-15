using Oracle.ManagedDataAccess.Client;
using System;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace WindowsFormsApp3
{
    public partial class RegisterForm : Form
    {
        public RegisterForm()
        {
            InitializeComponent();
        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            string username = txtUsername.Text.Trim();
            string password = txtPassword.Text.Trim();
            string confirmPassword = txtConfirmPassword.Text.Trim();
            string hoTen = txtHoTen.Text.Trim();
            string email = txtEmail.Text.Trim();
            string soDienThoai = txtSoDienThoai.Text.Trim();

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(hoTen))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc!");
                return;
            }
            if (password != confirmPassword)
            {
                MessageBox.Show("Mật khẩu nhập lại không khớp!");
                return;
            }

            using (var conn = DatabaseHelper.Get_Connect())
            {
                // Kiểm tra trùng username
                var checkCmd = new OracleCommand("SELECT COUNT(*) FROM NguoiDung WHERE TenDangNhap = :u", conn);
                checkCmd.Parameters.Add(new OracleParameter("u", username));
                int count = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (count > 0)
                {
                    MessageBox.Show("Tên đăng nhập đã tồn tại!");
                    return;
                }

                // Sinh salt + hash
                string salt = SecurityUtils.GenerateSalt();
                string hashedPassword = SecurityUtils.HashPassword(password, salt);

                // Thêm user mới với đầy đủ thông tin
                var insertCmd = new OracleCommand(@"INSERT INTO NguoiDung (MaNguoiDung, TenDangNhap, MatKhauHash, Salt, HoTen, Email, SoDienThoai, VaiTro, TrangThai)
                    VALUES (:id, :u, :p, :s, :ht, :em, :sdt, 'SINHVIEN', 1)", conn);

                insertCmd.Parameters.Add(new OracleParameter("id", Guid.NewGuid().ToString()));
                insertCmd.Parameters.Add(new OracleParameter("u", username));
                insertCmd.Parameters.Add(new OracleParameter("p", hashedPassword));
                insertCmd.Parameters.Add(new OracleParameter("s", salt));
                insertCmd.Parameters.Add(new OracleParameter("ht", hoTen));
                insertCmd.Parameters.Add(new OracleParameter("em", email));
                insertCmd.Parameters.Add(new OracleParameter("sdt", soDienThoai));

                insertCmd.ExecuteNonQuery();
            }

            MessageBox.Show("Đăng ký thành công! Vui lòng đăng nhập.");
            this.Close();
        }
    }
}
