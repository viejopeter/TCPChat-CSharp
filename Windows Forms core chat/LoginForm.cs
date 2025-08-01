using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            panel2.Hide();
        }
        private void btn_login_Click(object sender, EventArgs e)
        {
            btn_enter.Text = "Login";
            lb_welcome.Text = "Welcome back!";
            label3.Hide();
            textBox3.Hide();
            panel1.Hide();
            panel2.Show();
        }
        private void btn_signup_Click(object sender, EventArgs e)
        {
            btn_enter.Text = "Sign Up";
            lb_welcome.Text = "Welcome to the Tic Tac Toe chat!";
            panel1.Hide();
            panel2.Show();
        }

        private void btn_enter_Click(object sender, EventArgs e)
        {
            var form1 = new Form1();
            this.Hide();
            form1.Show();
        }
    }
}
