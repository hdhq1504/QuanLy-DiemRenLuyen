using Oracle.ManagedDataAccess.Client;
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
            string host = txtHost.Text;
            string port = txtPort.Text;
            string sid = txtSid.Text;
            string user = txtUsername.Text;
            string pass = txtPassword.Text;

            DatabaseHelper.Set_DatabaseHelper(host, port, sid, user, pass);
            if (DatabaseHelper.Connect())
            {
                OracleConnection c = DatabaseHelper.Get_Connect();
                MessageBox.Show("Đăng nhập thành công\nServerSession: " + c.ServerVersion);

                this.Hide();
                MainForm mainForm = new MainForm();
                mainForm.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Đăng nhập thất bại");
            }
        }
    }
}
