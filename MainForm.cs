using System.Windows.Forms;

namespace WindowsFormsApp3
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnLogout_Click(object sender, System.EventArgs e)
        {
            SessionManager.Logout();
            this.Hide();
            new LoginForm().Show();
        }

        private void MainForm_Load(object sender, System.EventArgs e)
        {
            lblWelcome.Text = $"Chào mừng {SessionManager.Username}!";
        }
    }
}
