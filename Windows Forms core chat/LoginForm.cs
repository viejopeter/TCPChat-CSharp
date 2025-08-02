using System;
using System.Windows.Forms;
using static System.Windows.Forms.DataFormats;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Windows_Forms_Chat
{
    public partial class LoginForm : Form
    {
        private DatabaseManager db = new DatabaseManager();
        private TCPChatClient client;

        public LoginForm(TCPChatClient client)
        {
            InitializeComponent();
            panel2.Hide();
            this.client = client;
        }
        private void btn_login_Click(object sender, EventArgs e)
        {
            btn_enter.Text = "Login";
            lb_welcome.Text = "Welcome back!";
            label3.Hide();
            textBox3.Hide();
            panel1.Hide();
            panel2.Show();

            btn_switchToSignUp.Show();
            btn_switchToLogin.Hide();
        }
        private void btn_signup_Click(object sender, EventArgs e)
        {
            btn_enter.Text = "Sign Up";
            lb_welcome.Text = "Welcome to the Tic Tac Toe chat!";
            label3.Show();
            textBox3.Show();
            panel1.Hide();
            panel2.Show();

            btn_switchToSignUp.Hide();
            btn_switchToLogin.Show();
        }

        private void btn_enter_Click(object sender, EventArgs e)
        {
            string username = textBox1.Text.Trim();      // Username textbox
            string password = textBox2.Text.Trim();      // Password textbox
            string confirmPassword = textBox3.Text.Trim(); // Confirm password textbox (used only for sign up)

            if (btn_enter.Text == "Login")
            {
                // Validate login fields
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                {
                    MessageBox.Show("Please enter both username and password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Try to log in
                string loginResult = db.LoginUser(username, password);
                if (loginResult == "Login successful.")
                {
                    this.DialogResult = DialogResult.OK;  // Signal success
                    client.clientSocket.State = ClientState.Chatting; // Set client state to Chatting
                    this.Close();  // Close LoginForm and return to Form1
                }
                else
                {
                    textBox1.Text = ""; // Clear username textbox
                    textBox2.Text = ""; // Clear password textbox
                    MessageBox.Show(loginResult, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (btn_enter.Text == "Sign Up")
            {
                // Validate signup fields
                if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(confirmPassword))
                {
                    MessageBox.Show("Please complete all fields: username, password, and confirm password.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Try to sign up
                string signupResult = db.SignUpUser(username, password, confirmPassword);

                if (signupResult == "Sign up successful.")
                {
                    this.DialogResult = DialogResult.OK;  // Signal success
                    client.clientSocket.State = ClientState.Chatting; // Set client state to Chatting
                    this.Close();  // Close LoginForm and return to Form1
                }
                else if (signupResult == "Username already exists.")
                {
                    // Clear fields if username already exists
                    textBox1.Clear(); // username
                    textBox2.Clear(); // password
                    textBox3.Clear(); // confirm password
                    textBox1.Focus();

                    MessageBox.Show(signupResult, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (signupResult == "Passwords do not match.")
                {
                    // Clear only passwords if they do not match
                    textBox2.Clear();
                    textBox3.Clear();
                    textBox2.Focus();

                    MessageBox.Show(signupResult, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
        private void btn_switchToSignUp_Click(object sender, EventArgs e)
        {
            textBox1.Text = ""; // Clear username textbox
            textBox2.Text = ""; // Clear password textbox
            btn_signup_Click(sender, e);
        }

        private void btn_switchToLogin_Click(object sender, EventArgs e)
        {
            textBox1.Text = ""; // Clear username textbox
            textBox2.Text = ""; // Clear password textbox
            textBox3.Text = ""; // Clear password textbox
            btn_login_Click(sender, e);
        }
        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // If LoginForm is closed without success, exit the application
            if (this.DialogResult != DialogResult.OK)
            {
                Application.Exit();  // Close the entire app
            }
        }
    }
}
