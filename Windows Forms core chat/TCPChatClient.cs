using System;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    public class TCPChatClient : TCPChatBase
    {
        // Main socket used to connect to the server
        public Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        
        // Wrapper class holding socket and client
        public ClientSocket clientSocket = new ClientSocket();
        public Form1 mainForm;

        public int serverPort;
        public string serverIP;

        // Factory method to create an instance of the client with proper setup
        public static TCPChatClient CreateInstance(int port, int serverPort, string serverIP, TextBox chatTextBox, string username_txt, Form1 form)
        {
            TCPChatClient tcp = null;
            // Validate ports and IP, and ensure chatTextBox is not null
            if (port > 0 && port < 65535 &&
                serverPort > 0 && serverPort < 65535 &&
                serverIP.Length > 0 &&
                chatTextBox != null)
            {
                tcp = new TCPChatClient();
                tcp.port = port;
                tcp.serverPort = serverPort;
                tcp.serverIP = serverIP;
                tcp.chatTextBox = chatTextBox;
                tcp.clientSocket.socket = tcp.socket;
                tcp.clientSocket.username = username_txt;
                tcp.clientSocket.State = ClientState.Login; // Set initial state login
                tcp.mainForm = form; // Store the reference

            }

            return tcp;
        }
        // Attempt to connect to the server, retrying until successful
        public void ConnectToServer()
        {
            int attempts = 0;

            while (!socket.Connected)
            {
                try
                {
                    attempts++;
                    SetChat("Connection attempt " + attempts);
                    // Connect to the server using provided IP and port
                    socket.Connect(serverIP, serverPort);
                }
                catch (SocketException)
                {
                    // Clear chat box on connection failure
                    chatTextBox.Text = "";
                }
            }

            // Show successful connection
            AddToChat("Connected");

            // Begin listening for incoming data from the server
            clientSocket.socket.BeginReceive(clientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, clientSocket);
        }
        // Sends a plain text message to the server
        public void SendString(string text)
        {
            byte[] buffer = Encoding.ASCII.GetBytes(text);
            socket.Send(buffer, 0, buffer.Length, SocketFlags.None);
        }

        // Callback for receiving data from the server
        public void ReceiveCallback(IAsyncResult AR)
        {
            ClientSocket currentClientSocket = (ClientSocket)AR.AsyncState;

            int received;

            try
            {
                received = currentClientSocket.socket.EndReceive(AR);
            }
            catch (SocketException)
            {
                AddToChat("Client forcefully disconnected");
                // Close socket if disconnected
                currentClientSocket.socket.Close();
                return;
            }
            // Copy received data into a new byte array
            byte[] recBuf = new byte[received];
            Array.Copy(currentClientSocket.buffer, recBuf, received);
            
            // Convert received bytes to string
            string text = Encoding.ASCII.GetString(recBuf);
            Console.WriteLine("Received Text: " + text);

            // Split by newline to handle multiple commands in one receive
            string[] commands = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var cmdRaw in commands)
            {
                string cmd = cmdRaw.Trim();
                // Handle specific server commands or response
                if (cmd == "!clear_chat")
                {
                    // Clear the chat UI from the client side
                    chatTextBox.Invoke((Action)(() => chatTextBox.Clear()));
                    AddToChat("Chat has been cleared.");
                }
                else if (cmd == "Username already taken")
                {
                    MessageBox.Show("Username already exists. You will be disconnected.", "Username Conflict", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else if (cmd == "Username accepted successfully")
                {
                    MessageBox.Show("Username changed successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (cmd == "!player1")
                {
                    clientSocket.State = ClientState.Playing;
                    AddToChat("Welcome to Tic Tac Toe game: You are player 1");
                    // Store locally: you are player 1 (e.g., cross)
                }
                else if (cmd == "!player2")
                {
                    clientSocket.State = ClientState.Playing;
                    AddToChat("Welcome to Tic Tac Toe game: You are player 2");
                    // Store locally: you are player 2 (e.g., naught)
                }
                else if (cmd == "!yourturn")
                {
                    clientSocket.State = ClientState.Playing;
                    AddToChat("[Game] It's your turn!");
                    mainForm?.Invoke((Action)(() => mainForm.EnableTicTacToeBoard()));

                }
                else if (cmd == "!wait")
                {
                    AddToChat("[Game] Wait for your turn...");
                    mainForm?.Invoke((Action)(() => mainForm.DisableTicTacToeBoard()));

                }
                else if (cmd.StartsWith("!board "))
                {
                    string boardState = text.Substring(7);
                    mainForm?.Invoke((Action)(() => mainForm.UpdateTicTacToeBoard(boardState)));
                }
                else if (cmd == "!win")
                {
                    AddToChat("[Game] You won the game!");
                }
                else if (cmd == "!lose")
                {
                    AddToChat("[Game] You lost the game.");
                }
                else if (cmd == "!draw")
                {
                    AddToChat("[Game] The game is a draw.");
                }
                else if (cmd == "!chatstate")
                {
                    clientSocket.State = ClientState.Chatting;
                    mainForm?.Invoke((Action)(() => mainForm.DisableTicTacToeBoard()));
                    //Reset the local board
                    mainForm?.Invoke((Action)(() => mainForm.UpdateTicTacToeBoard("_________")));
                }
                else
                {
                    // General message received from server
                    AddToChat(cmd);
                }
            }

            // Continue listening for more data from the server
            currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
        }
        // Close the client's socket connection
        public void Close()
        {
            socket.Close();
        }
    }

}
