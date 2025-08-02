using System;
using System.Drawing;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    public partial class Form1 : Form
    {
        TicTacToe ticTacToe = new TicTacToe(); // Tic Tac Toe game logic
        TCPChatServer server = null; // Server instance
        TCPChatClient client = null; // Client instance
        TabPage tabToDisable; // Used to disable unused tabs (UI cleanup)

        public Form1()
        {
            InitializeComponent();

            // Hide unused elements initially
            lb_username.Visible = false;
            username_txt.Visible = false;
            change_username_btn.Visible = false;
            groupBox1.Visible = false;
            ChatTextBox.TabStop = false;
            list_users_btn_ser.Visible = false;
            button10.Visible = false;
            button11.Visible = false;

            // Make chat textbox read only and cleaner
            ChatTextBox.ReadOnly = true;
            ChatTextBox.ReadOnly = true;
            ChatTextBox.BackColor = Color.White;

            // Set focus to server IP field after form is shown
            this.Shown += Form1_Shown;

        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            ServerIPTextBox.Focus();
        }

        // Check if user can host or join (only one at a time)
        public bool CanHostOrJoin()
        {
            if (server == null && client == null)
                return true;
            else
                return false;
        }
        // Start as server
        private void HostButton_Click(object sender, EventArgs e)
        {
            if (CanHostOrJoin())
            {
                try
                {
                    int port = int.Parse(MyPortTextBox.Text);

                    // Create server instance
                    server = TCPChatServer.createInstance(port, ChatTextBox);

                    if (server == null)
                        throw new Exception("Incorrect port value!");

                    server.SetupServer();
                    // Database manager instance
                    DatabaseManager dbManager = new DatabaseManager();

                    // Update UI
                    tabToDisable = tabControl1.TabPages[1];
                    tabControl1.TabPages.Remove(tabToDisable);
                    HostButton.Visible = false;
                    serverPortTextBox.ReadOnly = true;
                    ServerIPTextBox.ReadOnly = true;
                    TypeTextBox.Focus();

                    // Show server-only buttons
                    list_users_btn_ser.Visible = true;
                    button10.Visible = true;
                    button11.Visible = true;
                }
                catch (SocketException se)
                {
                    // Port already in use
                    MessageBox.Show("You cannot be server.", "Port In Use", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    Environment.Exit(0);
                }
                catch (FormatException)
                {
                    MessageBox.Show("Please enter a valid port number.", "Invalid Port", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(0);
                }
                catch (Exception ex)
                {
                    // General error
                    ChatTextBox.AppendText("Error: " + ex.Message + Environment.NewLine);
                    Environment.Exit(0);
                }
            }
        }
        // Join as client
        private void JoinButton_Click(object sender, EventArgs e)
        {
            if (CanHostOrJoin())
            {
                try
                {

                    int port = int.Parse(MyPortTextBox.Text);
                    int serverPort = int.Parse(serverPortTextBox.Text);
                    client = TCPChatClient.CreateInstance(port, serverPort, ServerIPTextBox.Text, ChatTextBox, username_txt.Text);

                    if (client == null)
                        //thrown exceptions should exit the try and land in next catch
                        throw new Exception("Incorrect port value!");

                    client.ConnectToServer();

                    // Hide Form1 before opening LoginForm
                    this.Hide();

                    // Open LoginForm as a modal
                    var loginForm = new LoginForm(client);
                    var result = loginForm.ShowDialog();  // Blocks until LoginForm is closed

                    if (result == DialogResult.OK)
                    {

                        // Update UI as per the login success
                        JoinButton.Visible = false;
                        MyPortTextBox.ReadOnly = true;
                        label1.Visible = true;

                        lb_username.Visible = true;
                        username_txt.Visible = true;
                        change_username_btn.Visible = true;
                        groupBox1.Visible = true;

                        tabToDisable = tabControl1.TabPages[0];
                        tabControl1.TabPages.Remove(tabToDisable);

                        TypeTextBox.Focus();

                        // If you want to re-show the Form1, do it after the login
                        this.Show();
                    }
                    else
                    {
                        // If LoginForm was closed or login failed, close the application
                        Application.Exit();
                    }
                }
                catch (Exception ex)
                {
                    client = null;
                    ChatTextBox.Text += "Error: " + ex;
                    ChatTextBox.AppendText(Environment.NewLine);
                }
            }
        }
        // Change username (client sends command to server)
        private void change_username_btn_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(username_txt.Text))
            {
                MessageBox.Show("The field username cannot be empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                username_txt.Focus();
                return;
            }
            else
            {
                client.SendString("!username " + username_txt.Text);
                username_txt.Text = "";
            }
        }
        // Send message or server command
        private void SendButton_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrWhiteSpace(TypeTextBox.Text))
            {
                MessageBox.Show("The field chat cannot be empty.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                TypeTextBox.Focus();
                return;

            }
            else
            {

                string input = TypeTextBox.Text;

                if (client != null)
                    client.SendString(input);
                else if (server != null)
                {
                    //it’s a command sever side
                    if (input.StartsWith("!"))
                    {
                        // Regular server message
                        server.AddToChat($"[Server]: {input}");
                        server.HandleServerCommand(input);
                    }
                    else
                    {
                        // Regular server message
                        server.AddToChat($"[Server]: {input}");
                        server.SendToAll($"[Server]: {input}", null);
                    }
                }

                TypeTextBox.Clear();
                TypeTextBox.Focus();

            }
        }
        // On form load, initialize Tic Tac Toe buttons
        private void Form1_Load(object sender, EventArgs e)
        {
            //On form loaded
            ticTacToe.buttons.Add(button1);
            ticTacToe.buttons.Add(button2);
            ticTacToe.buttons.Add(button3);
            ticTacToe.buttons.Add(button4);
            ticTacToe.buttons.Add(button5);
            ticTacToe.buttons.Add(button6);
            ticTacToe.buttons.Add(button7);
            ticTacToe.buttons.Add(button8);
            ticTacToe.buttons.Add(button9);
            ServerIPTextBox.Focus();
        }
        // Attempt a move in Tic Tac Toe
        private void AttemptMove(int i)
        {
            if (ticTacToe.myTurn)
            {
                bool validMove = ticTacToe.SetTile(i, ticTacToe.playerTileType);
                if (validMove)
                {
                    //tell server about it
                    //ticTacToe.myTurn = false;//call this too when ready with server
                }
                //example, do something similar from server
                GameState gs = ticTacToe.GetGameState();
                if (gs == GameState.crossWins)
                {
                    ChatTextBox.AppendText("X wins!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
                if (gs == GameState.naughtWins)
                {
                    ChatTextBox.AppendText(") wins!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
                if (gs == GameState.draw)
                {
                    ChatTextBox.AppendText("Draw!");
                    ChatTextBox.AppendText(Environment.NewLine);
                    ticTacToe.ResetBoard();
                }
            }
        }
        // Tic Tac Toe button click handlers
        private void button1_Click(object sender, EventArgs e)
        {
            AttemptMove(0);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AttemptMove(1);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AttemptMove(2);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AttemptMove(3);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AttemptMove(4);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AttemptMove(5);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AttemptMove(6);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AttemptMove(7);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            AttemptMove(8);
        }
        // Client-side command buttons
        private void commands_btn_Click(object sender, EventArgs e)
        {
            client.SendString("!commands");
        }

        private void whoAmIButton_Click(object sender, EventArgs e)
        {
            client.SendString("!whoami");
        }

        private void about_btn_Click(object sender, EventArgs e)
        {
            client.SendString("!about");
        }

        private void list_users_btn_Click(object sender, EventArgs e)
        {
            client.SendString("!who");
        }

        private void btn_help_Click(object sender, EventArgs e)
        {
            client.SendString("!help");
        }

        private void button12_Click(object sender, EventArgs e)
        {
            client.SendString("!joke");
        }

        private void button13_Click(object sender, EventArgs e)
        {
            client.SendString("!time");
        }

        // Server-side command buttons
        private void list_users_btn_ser_Click(object sender, EventArgs e)
        {
            server.HandleServerCommand("!who");
        }

        private void button10_Click(object sender, EventArgs e)
        {
            server.HandleServerCommand("!commands");
        }

        private void button11_Click(object sender, EventArgs e)
        {
            server.HandleServerCommand("!mods");
        }

        // Confirm before exiting
        private void exit_btn_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to exit the chat?", "Confirm Exit",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                client.SendString("!exit");
            }
        }

        // Clear chat box
        private void clear_btn_Click(object sender, EventArgs e)
        {
            ChatTextBox.Clear();
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}
