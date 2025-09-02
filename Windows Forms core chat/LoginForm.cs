using System;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    // LoginForm handles user authentication and registration UI logic
    public partial class LoginForm : Form
    {
        // Database manager for user authentication and registration
        private DatabaseManager db = new DatabaseManager();
        // Reference to the TCP chat client for sending login/signup info to server
        private TCPChatClient _client;

        // Constructor: initializes the form and hides the login/signup panel
        public LoginForm(TCPChatClient client)
        {
            InitializeComponent();
            panel2.Hide();
            this._client = client;
        }

        // Switches UI to login mode
        private void btn_login_Click(object sender, EventArgs e)
        {
            btn_enter.Text = "Login";
            lb_welcome.Text = "Welcome back!";
            label3.Hide(); // Hide confirm password label
            textBox3.Hide(); // Hide confirm password textbox
            panel1.Hide(); // Hide initial panel
            panel2.Show(); // Show login/signup panel

            btn_switchToSignUp.Show();
            btn_switchToLogin.Hide();
        }

        // Switches UI to sign up mode
        private void btn_signup_Click(object sender, EventArgs e)
        {
            btn_enter.Text = "Sign Up";
            lb_welcome.Text = "Welcome to the Tic Tac Toe chat!";
            label3.Show(); // Show confirm password label
            textBox3.Show(); // Show confirm password textbox
            panel1.Hide();
            panel2.Show();

            btn_switchToSignUp.Hide();
            btn_switchToLogin.Show();
        }

        // Handles login or sign up when user clicks enter
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

                // Check if the user is not already authenticated (not logged in)
                if (!db.IsUserAuthenticated(username))
                {
                    // Try to log in with the provided username and password
                    string loginResult = db.LoginUser(username, password);
                    // If login is successful
                    if (loginResult == "Login successful.")
                    {
                        // Set the dialog result to OK to signal success to the main form
                        this.DialogResult = DialogResult.OK;
                        // Notify the server that the user has logged in, including their username and chat state
                        _client.SendString("!login username=" + username + ";state=" + ClientState.Chatting);
                        // Close the login form and return control to Form1
                        this.Close(); 
                    }
                    else
                    {
                        // Clear the username and password fields
                        textBox1.Text = "";
                        textBox2.Text = "";
                        // Show an error message with the reason for login failure
                        MessageBox.Show(loginResult, "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    // If the user is already authenticated, show a warning
                    MessageBox.Show("This account is already logged", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Clear the username and password fields
                    textBox1.Text = "";
                    textBox2.Text = "";
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

                // Check if the username exists AND is currently authenticated
                if (db.IsUserAuthenticated(username))
                {
                    MessageBox.Show("This account is already registered and currently logged in.", "Sign Up Blocked", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    // Clear fields if username already exists
                    textBox1.Clear(); // username
                    textBox2.Clear(); // password
                    textBox3.Clear(); // confirm password
                    textBox1.Focus();
                    return;
                }

                // Try to sign up
                string signupResult = db.SignUpUser(username, password, confirmPassword);

                if (signupResult == "Sign up successful.")
                {
                    this.DialogResult = DialogResult.OK;  // Signal success
                    // Send a command to the server to update the username and state
                    _client.SendString("!login username=" + username + ";state=" + ClientState.Chatting);
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
                    textBox2.Clear(); // password
                    textBox3.Clear(); // confirm password
                    textBox2.Focus();

                    MessageBox.Show(signupResult, "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        // Switches UI to sign up mode and clears fields
        private void btn_switchToSignUp_Click(object sender, EventArgs e)
        {
            textBox1.Text = ""; // Clear username textbox
            textBox2.Text = ""; // Clear password textbox
            btn_signup_Click(sender, e);
        }

        // Switches UI to login mode and clears fields
        private void btn_switchToLogin_Click(object sender, EventArgs e)
        {
            textBox1.Text = ""; // Clear username textbox
            textBox2.Text = ""; // Clear password textbox
            textBox3.Text = ""; // Clear password textbox
            btn_login_Click(sender, e);
        }

        // Handles form close event: exits app if login/signup not successful
        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            // If LoginForm is closed without success, exit the application
            if (this.DialogResult != DialogResult.OK)
            {
                //db.LogoutUser(username);
                Application.Exit();  // Close the entire app
            }
        }
    }
}
