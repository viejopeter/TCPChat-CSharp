using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    // Main server class for chat and Tic-Tac-Toe game management
    public class TCPChatServer : TCPChatBase
    {
        // Database manager for user authentication and score tracking
        private DatabaseManager db = new DatabaseManager();
        // Main server socket
        public Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        // List of all connected clients
        public List<ClientSocket> clientSockets = new List<ClientSocket>();
        // String for connected users (used for display)
        string lsConnectedUsers = "";

        // Tic-Tac-Toe game state
        private ClientSocket player1 = null; // Player 1 socket
        private ClientSocket player2 = null; // Player 2 socket
        private ClientSocket currentTurn = null; // Whose turn is it
        private TicTacToe serverTicTacToe = new TicTacToe(); // Server-side board
        private bool gameInProgress = false; // Is a game running?
        private TileType player1Tile = TileType.cross; // Player 1's tile type
        private TileType player2Tile = TileType.naught; // Player 2's tile type

        // Factory method to create server instance with a port and UI text box
        public static TCPChatServer createInstance(int port, TextBox chatTextBox)
        {
            TCPChatServer tcp = null;
            // Setup if port within range and valid chat box given
            if (port > 0 && port < 65535 && chatTextBox != null)
            {
                tcp = new TCPChatServer();
                tcp.port = port;
                tcp.chatTextBox = chatTextBox;
            }
            // Return empty if user did not enter useful details
            return tcp;
        }

        // Initializes and starts the TCP server
        public void SetupServer()
        {
            chatTextBox.Text += "Setting up server..." + Environment.NewLine;
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            serverSocket.Listen(0); // Start listening for connections
            serverSocket.BeginAccept(AcceptCallback, this); // Accept first client
            chatTextBox.Text += "Server setup complete " + Environment.NewLine;
            // Log out all users in the database
            db.LogoutAllUsers();
        }

        // Close all client connections and server socket
        public void CloseAllSockets()
        {
            foreach (ClientSocket clientSocket in clientSockets)
            {
                clientSocket.socket.Shutdown(SocketShutdown.Both);
                clientSocket.socket.Close();
            }
            // Log out all users in the database
            db.LogoutAllUsers(); 
            clientSockets.Clear();
            serverSocket.Close();
        }

        // Callback for new client connection
        public void AcceptCallback(IAsyncResult AR)
        {
            Socket joiningSocket;
            try
            {
                joiningSocket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException)
            {
                return; // Socket was closed
            }
            // Create new client object and assign a temporary username 
            ClientSocket newClientSocket = new ClientSocket();
            newClientSocket.socket = joiningSocket;
            newClientSocket.username = $"User-{newClientSocket.ShortId}";
            clientSockets.Add(newClientSocket);
            // Start listening for incoming data from this client
            joiningSocket.BeginReceive(newClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, newClientSocket);
            AddToChat("Client connected, waiting for request...");
            // Continue accepting more clients
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        // Handles received messages and commands from a client
        public void ReceiveCallback(IAsyncResult AR)
        {
            ClientSocket currentClientSocket = (ClientSocket)AR.AsyncState;
            int received;
            string oldUsername = "";
            string formattedMessage = "";
            try
            {
                received = currentClientSocket.socket.EndReceive(AR);
            }
            catch (SocketException)
            {
                // Handle client disconnect and update database
                db.LogoutUser(currentClientSocket.username);
                // Handle Tic-Tac-Toe disconnect logic
                if (gameInProgress && (currentClientSocket == player1 || currentClientSocket == player2))
                {
                    // If a player disconnects during a game, award win to the other
                    ClientSocket winner = null;
                    ClientSocket loser = currentClientSocket;
                    if (currentClientSocket == player1 && player2 != null)
                        winner = player2;
                    else if (currentClientSocket == player2 && player1 != null)
                        winner = player1;
                    if (winner != null)
                    {
                        winner.socket.Send(Encoding.ASCII.GetBytes("!win\n"));
                        winner.socket.Send(Encoding.ASCII.GetBytes("Opponent disconnected. You win by default.\n"));
                        db.AddWin(winner.username);
                        foreach (var c in clientSockets)
                        {
                            c.socket.Send(Encoding.ASCII.GetBytes("!chatstate\n"));
                        }
                        winner.State = ClientState.Chatting;
                    }
                    if (loser != null)
                    {
                        db.AddLoss(loser.username);
                    }
                    // Reset game state
                    gameInProgress = false;
                    player1 = null;
                    player2 = null;
                    currentTurn = null;
                    serverTicTacToe.ResetBoard();
                }
                // Handle waiting-for-game disconnect
                else if (!gameInProgress)
                {
                    if (currentClientSocket == player1)
                    {
                        player1 = null;
                    }
                    else if (currentClientSocket == player2)
                    {
                        player2 = null;
                    }
                }
                AddToChat("Client forcefully disconnected");
                currentClientSocket.socket.Close();
                clientSockets.Remove(currentClientSocket);
                return;
            }
            // Parse received data
            byte[] recBuf = new byte[received];
            Array.Copy(currentClientSocket.buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            string[] parts = text.Trim().Split(' ', 2);
            string cmd = parts[0].ToLower(); // Command
            string targetUsername = parts.Length > 1 ? parts[1].Trim() : null; // Parameter
            string messageLogServer = $"[Server Log] User '{currentClientSocket.username}'";

            // Handle chat/game commands
            if (cmd == "!commands")
            {
                // Send list of commands to client
                byte[] data = Encoding.ASCII.GetBytes("Commands are !commands !about !who !whoami !username !whisper !help !clear !kick !exit ");
                currentClientSocket.socket.Send(data);
                AddToChat($"{messageLogServer} requested the list of commands.");
            }
            else if (cmd == "!username")
            {
                // Handle username change and validation
                bool existUsername = clientSockets.Any(otherClient =>
                    otherClient != currentClientSocket &&
                    otherClient.username.Equals(targetUsername, StringComparison.OrdinalIgnoreCase));
                bool existInDb = db.UsernameExists(targetUsername) &&
                                 !currentClientSocket.username.Equals(targetUsername, StringComparison.OrdinalIgnoreCase);
                if (existUsername || existInDb)
                {
                    byte[] rejection = Encoding.ASCII.GetBytes("Username already taken\n");
                    currentClientSocket.socket.Send(rejection);
                    db.LogoutUser(currentClientSocket.username);
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    AddToChat($"{messageLogServer} was disconnected for trying to use an existing username: '{targetUsername}'");
                    SendToAll($"User {currentClientSocket.username} was disconnected", currentClientSocket);
                    return;
                }
                else
                {
                    if (currentClientSocket.username == targetUsername)
                    {
                        byte[] sameNameMessage = Encoding.ASCII.GetBytes("You're already using this username.");
                        currentClientSocket.socket.Send(sameNameMessage);
                    }
                    else
                    {
                        oldUsername = currentClientSocket.username;
                        currentClientSocket.username = targetUsername;
                        db.UpdateUsername(oldUsername, targetUsername);
                        formattedMessage = $"User '{oldUsername}' changed username to '{targetUsername}' successfully";
                        byte[] data = Encoding.ASCII.GetBytes("Username accepted successfully\n");
                        currentClientSocket.socket.Send(data);
                        AddToChat(formattedMessage);
                        SendToAll(formattedMessage, currentClientSocket);
                    }
                }
            }
            else if (cmd == "!who") // Send back a list connected user (usernames)
            {
                lsConnectedUsers = GetConnectedUsersList();
                byte[] data = Encoding.ASCII.GetBytes("Connected users:" + Environment.NewLine + lsConnectedUsers);
                currentClientSocket.socket.Send(data);
                AddToChat($"{messageLogServer} used !who command");
            }
            else if (cmd == "!about")// Client requested information about the server
            {
                string aboutMessage = "Server Information: " + Environment.NewLine;
                aboutMessage += "Editor: Pedro Q " + Environment.NewLine;
                aboutMessage += "Purpose: TCP Chat server for connecting users and sharing messages " + Environment.NewLine;
                aboutMessage += "Year of Development: 2025 " + Environment.NewLine;
                byte[] data = Encoding.ASCII.GetBytes(aboutMessage);
                currentClientSocket.socket.Send(data);
                AddToChat($"{messageLogServer} used !about command");
            }
            else if (cmd == "!whoami")
            {
                string currentUsername = currentClientSocket.username;
                string reply = $"Your current username is: {currentUsername}";
                byte[] data = Encoding.ASCII.GetBytes(reply);
                currentClientSocket.socket.Send(data);
                AddToChat($"{messageLogServer} used !whoami command");
            }
            else if (cmd == "!whisper")
            {
                // Handle private message to another user
                int spaceIndex = targetUsername.IndexOf(' ');
                if (spaceIndex == -1)
                {
                    byte[] errorMsg = Encoding.ASCII.GetBytes("Invalid whisper format. Use: !whisper [username] [message]");
                    currentClientSocket.socket.Send(errorMsg);
                }
                else
                {
                    string whisper_targetUsername = targetUsername.Substring(0, spaceIndex);
                    string messageToSend = targetUsername.Substring(spaceIndex + 1);
                    if (currentClientSocket.username != null &&
                        currentClientSocket.username.Equals(whisper_targetUsername, StringComparison.OrdinalIgnoreCase))
                    {
                        byte[] selfWhisperMsg = Encoding.ASCII.GetBytes("You cannot whisper to yourself.");
                        currentClientSocket.socket.Send(selfWhisperMsg);
                        AddToChat($"{messageLogServer} tried to whisper to themselves.");
                    }
                    else
                    {
                        ClientSocket targetClient = FindClientByUsername(whisper_targetUsername);
                        if (targetClient != null)
                        {
                            string whisperMessage = $"[Whisper from {currentClientSocket.username}]: {messageToSend}";
                            byte[] data = Encoding.ASCII.GetBytes(whisperMessage);
                            targetClient.socket.Send(data);
                            byte[] confirmation = Encoding.ASCII.GetBytes($"[Whisper to {whisper_targetUsername}]: {messageToSend}");
                            currentClientSocket.socket.Send(confirmation);
                            AddToChat($"{messageLogServer} whispered to {whisper_targetUsername}: {messageToSend}");
                        }
                        else
                        {
                            byte[] notFoundMsg = Encoding.ASCII.GetBytes($"User '{whisper_targetUsername}' not found.");
                            currentClientSocket.socket.Send(notFoundMsg);
                            AddToChat($"{messageLogServer} tried to whisper to non existent user '{whisper_targetUsername}'");
                        }
                    }
                }
            }
            else if (cmd == "!help")
            {
                string helpText =
                    "Available Commands: " + Environment.NewLine +
                    "!commands = List all commands " + Environment.NewLine +
                    "!about = Info about the server " + Environment.NewLine +
                    "!who = List connected users " + Environment.NewLine +
                    "!whoami = Show your username " + Environment.NewLine +
                    "!username [newName] = Change username " + Environment.NewLine +
                    "!whisper [username] [msg] = Send private message " + Environment.NewLine +
                    "!help = Details of the command list " + Environment.NewLine +
                    "!clear = Clear the chat window " + Environment.NewLine +
                    "!kick = Disconnects a user from the chat (only available to moderators)" +
                    "!exit = Disconnect from chat";
                currentClientSocket.socket.Send(Encoding.ASCII.GetBytes(helpText));
                AddToChat($"{messageLogServer} used !help command");
            }
            else if (cmd == "!clear")
            {
                currentClientSocket.socket.Send(Encoding.ASCII.GetBytes("!clear_chat\n"));
                AddToChat($"{messageLogServer} used !clear command");
            }
            else if (cmd == "!kick")
            {
                // Moderator command to kick a user
                if (!currentClientSocket.IsModerator)
                {
                    currentClientSocket.socket.Send(Encoding.ASCII.GetBytes("[Server Log] Only moderators can kick users."));
                    AddToChat($"{messageLogServer} attempted to use the !kick command but is not a moderator.");
                }
                else
                {
                    ClientSocket targetUser = FindClientByUsername(targetUsername);
                    if (targetUser == null)
                    {
                        currentClientSocket.socket.Send(Encoding.ASCII.GetBytes($"[Server Log] user '{targetUsername}' not found."));
                    }
                    else if (targetUser == currentClientSocket)
                    {
                        currentClientSocket.socket.Send(Encoding.ASCII.GetBytes("[Server Log] You cannot kick yourself."));
                    }
                    else
                    {
                        targetUser.socket.Send(Encoding.ASCII.GetBytes("[Server Log] You have been kicked by a moderator."));
                        targetUser.socket.Shutdown(SocketShutdown.Both);
                        targetUser.socket.Close();
                        clientSockets.Remove(targetUser);
                        string message = $"[Server Log] user '{targetUser.username}' has been kicked by moderator '{currentClientSocket.username}'.";
                        AddToChat(message);
                        SendToAll(message, null);
                    }
                }
            }
            else if (cmd == "!joke")
            {
                // Send a random joke to all clients
                string[] jokes = {
                    "Why don't scientists trust atoms? Because they make up everything!",
                    "Why did the math book look sad? Because it had too many problems.",
                    "I told my wife she should embrace her mistakes. She gave me a hug.",
                    "Why cannot your nose be 12 inches long? Because then it would be a foot.",
                    "What do you call fake spaghetti? An impasta.",
                    "I bought some shoes from a drug dealer. I do not know what he laced them with, but I was tripping all day.",
                    "My friend says to me, - What rhymes with orange? -  I said, No it does not.",
                    "What did the ocean say to the beach? Nothing, it just waved.",
                    "I only know 25 letters of the alphabet. I do not know y.",
                    "What do you call a factory that makes good products? A satisfactory."
                };
                Random rand = new Random();
                string selectedJoke = jokes[rand.Next(jokes.Length)];
                string message = $"{currentClientSocket.username}: [Joke] {selectedJoke}";
                AddToChat($"{messageLogServer} used !joke command");
                SendToAll(message, null);
            }
            else if (cmd == "!exit") // Client wants to exit gracefully
            {
                db.LogoutUser(currentClientSocket.username);
                currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                currentClientSocket.socket.Close();
                clientSockets.Remove(currentClientSocket);
                AddToChat($"{messageLogServer} used !exit command and has been disconnected");
                SendToAll($"User {currentClientSocket.username} was disconnected", currentClientSocket);
                return;
            }
            else if (cmd == "!time")
            {
                string serverTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                byte[] data = Encoding.ASCII.GetBytes($"[Server Time] {serverTime}");
                currentClientSocket.socket.Send(data);
                AddToChat($"{messageLogServer} used !time command");
            }
            else if (cmd == "!scores")
            {
                // Send sorted leaderboard to client
                var scores = db.GetAllScoresSorted();
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("Username\tWins\tDraws\tLosses");
                foreach (var (username, wins, draws, losses) in scores)
                {
                    sb.AppendLine($"{username}\t{wins}\t{draws}\t{losses}");
                }
                currentClientSocket.socket.Send(Encoding.ASCII.GetBytes(sb.ToString()));
                AddToChat($"{messageLogServer} requested the scores.");
            }
            else if (cmd == "!login")
            {
                // Handle login command from client
                if (!string.IsNullOrWhiteSpace(targetUsername))
                {
                    var pairs = targetUsername.Split(';');
                    foreach (var pair in pairs)
                    {
                        var kv = pair.Split('=', 2);
                        if (kv.Length == 2)
                        {
                            var key = kv[0].Trim().ToLower();
                            var value = kv[1].Trim();
                            if (key == "username" && !string.IsNullOrWhiteSpace(value))
                            {
                                currentClientSocket.username = value;
                            }
                            else if (key == "state" && !string.IsNullOrWhiteSpace(value))
                            {
                                if (Enum.TryParse<ClientState>(value, true, out var state))
                                {
                                    currentClientSocket.State = state;
                                }
                            }
                        }
                    }
                    AddToChat($"[Server Log] Successfully login User: {currentClientSocket.username}, now is in {currentClientSocket.State} state ");
                }
            }
            else if (cmd == "!join")
            {
                // Handle join game request
                if (!gameInProgress)
                {
                    if (player1 == null)
                    {
                        player1 = currentClientSocket;
                        player1.State = ClientState.Playing;
                        player1Tile = TileType.cross;
                        currentClientSocket.socket.Send(Encoding.ASCII.GetBytes("!player1\n"));
                    }
                    else if (player2 == null && currentClientSocket != player1)
                    {
                        player2 = currentClientSocket;
                        player2.State = ClientState.Playing;
                        player2Tile = TileType.naught;
                        currentClientSocket.socket.Send(Encoding.ASCII.GetBytes("!player2\n"));
                        // Start the game
                        gameInProgress = true;
                        currentTurn = player1;
                        player1.socket.Send(Encoding.ASCII.GetBytes("!yourturn\n"));
                        player2.socket.Send(Encoding.ASCII.GetBytes("!wait\n"));
                    }
                    else
                    {
                        currentClientSocket.socket.Send(Encoding.ASCII.GetBytes("Game is full or already started."));
                    }
                }
                else
                {
                    currentClientSocket.socket.Send(Encoding.ASCII.GetBytes("Game already in progress."));
                }
            }
            else if (cmd == "!move")
            {
                // Handle Tic-Tac-Toe move from a player
                if (gameInProgress && currentClientSocket == currentTurn)
                {
                    if (int.TryParse(targetUsername, out int moveIndex))
                    {
                        // Update server-side board
                        var tileType = (currentTurn == player1) ? TileType.cross : TileType.naught;
                        bool valid = serverTicTacToe.SetTile(moveIndex, tileType);
                        if (valid)
                        {
                            // Broadcast new board state to all clients
                            string boardState = serverTicTacToe.GridToString();
                            foreach (var c in clientSockets)
                            {
                                c.socket.Send(Encoding.ASCII.GetBytes("!board " + boardState + "\n"));
                            }
                            // Check for win/draw
                            GameState gs = serverTicTacToe.GetGameState();
                            if (gs == GameState.crossWins || gs == GameState.naughtWins || gs == GameState.draw)
                            {
                                // Update scores in the database
                                string winner = null, loser = null;
                                if (gs == GameState.crossWins)
                                {
                                    if (player1Tile == TileType.cross)
                                    {
                                        winner = player1.username;
                                        loser = player2.username;
                                        player1.socket.Send(Encoding.ASCII.GetBytes("!win\n"));
                                        player2.socket.Send(Encoding.ASCII.GetBytes("!lose\n"));
                                    }
                                    else
                                    {
                                        winner = player2.username;
                                        loser = player1.username;
                                        player2.socket.Send(Encoding.ASCII.GetBytes("!win\n"));
                                        player1.socket.Send(Encoding.ASCII.GetBytes("!lose\n"));
                                    }
                                }
                                else if (gs == GameState.naughtWins)
                                {
                                    if (player1Tile == TileType.naught)
                                    {
                                        winner = player1.username;
                                        loser = player2.username;
                                        player1.socket.Send(Encoding.ASCII.GetBytes("!win\n"));
                                        player2.socket.Send(Encoding.ASCII.GetBytes("!lose\n"));
                                    }
                                    else
                                    {
                                        winner = player2.username;
                                        loser = player1.username;
                                        player2.socket.Send(Encoding.ASCII.GetBytes("!win\n"));
                                        player1.socket.Send(Encoding.ASCII.GetBytes("!lose\n"));
                                    }
                                }
                                else // draw
                                {
                                    player1.socket.Send(Encoding.ASCII.GetBytes("!draw\n"));
                                    player2.socket.Send(Encoding.ASCII.GetBytes("!draw\n"));
                                }
                                if (gs == GameState.crossWins || gs == GameState.naughtWins)
                                {
                                    db.AddWin(winner);
                                    db.AddLoss(loser);
                                }
                                else if (gs == GameState.draw)
                                {
                                    db.AddDraw(player1.username);
                                    db.AddDraw(player2.username);
                                }
                                // Inform all clients to return to chat state
                                foreach (var c in clientSockets)
                                {
                                    c.socket.Send(Encoding.ASCII.GetBytes("!chatstate\n"));
                                }
                                // Reset game state
                                gameInProgress = false;
                                player1.State = ClientState.Chatting;
                                player2.State = ClientState.Chatting;
                                player1 = null;
                                player2 = null;
                                currentTurn = null;
                                serverTicTacToe.ResetBoard();
                            }
                            else
                            {
                                // Switch turn
                                currentTurn = (currentTurn == player1) ? player2 : player1;
                                currentTurn.socket.Send(Encoding.ASCII.GetBytes("!yourturn\n"));
                                (currentTurn == player1 ? player2 : player1).socket.Send(Encoding.ASCII.GetBytes("!wait\n"));
                            }
                        }
                    }
                }
            }
            else
            {
                // General chat message
                formattedMessage = $"{currentClientSocket.username}: {text}";
                AddToChat(formattedMessage);
                SendToAll(formattedMessage, currentClientSocket);
            }
            // Continue listening for more data from this client
            currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
        }

        // Handles server-side admin commands (for server UI)
        public void HandleServerCommand(string command)
        {
            string[] parts = command.Trim().Split(' ', 2);
            string cmd = parts[0].ToLower();
            string targetUsername = parts.Length > 1 ? parts[1].Trim() : null;
            if (cmd == "!mod")
            {
                ClientSocket targetUser = FindClientByUsername(targetUsername);
                if (targetUser != null)
                {
                    if (targetUser.IsModerator)
                    {
                        targetUser.IsModerator = false;
                        SendToAll($"[Server] {targetUsername} has been demoted from moderator.", null);
                        AddToChat($"[Server Log] Demoted {targetUsername} from moderator.");
                    }
                    else
                    {
                        targetUser.IsModerator = true;
                        SendToAll($"[Server] {targetUsername} has been promoted to moderator.", null);
                        AddToChat($"[Server Log] Promoted {targetUsername} to moderator.");
                    }
                }
                else
                {
                    AddToChat($"[Server] User '{targetUsername}' not found.");
                }
            }
            else if (cmd == "!mods")
            {
                var mods = clientSockets.Where(c => c.IsModerator).Select(c => c.username).ToList();
                if (mods.Count == 0)
                {
                    AddToChat("[Server Log] No moderators currently assigned.");
                }
                else
                {
                    AddToChat("[Server Log] Current moderators:");
                    foreach (string mod in mods)
                        AddToChat($" - {mod}");
                }
            }
            else if (cmd == "!who")
            {
                lsConnectedUsers = GetConnectedUsersList();
                AddToChat("Connected users:" + Environment.NewLine + lsConnectedUsers); 
            }
            else if (cmd == "!commands")
            {
                string helpText =
                   "Available Commands: " + Environment.NewLine +
                   "!who = List connected users " + Environment.NewLine +
                   "!commands = List all commands " + Environment.NewLine +
                   "!mod = Promote and Demote moderator using [username] " + Environment.NewLine +
                   "!mods = List all moderators " + Environment.NewLine;
                AddToChat(helpText);
            }
            else
            {
                AddToChat("[Server Log] Unknown server command.");
            }
        }

        // Returns all connected usernames as a string
        public string GetConnectedUsersList()
        {
            if (clientSockets.Count == 0)
                return "No users connected";
            StringBuilder sb = new StringBuilder();
            foreach (ClientSocket c in clientSockets)
            {
                if (!string.IsNullOrEmpty(c.username))
                    sb.AppendLine(c.username);
            }
            if (string.IsNullOrWhiteSpace(sb.ToString()))
                return "No users connected";
            return sb.ToString().TrimEnd('\r', '\n');
        }

        // Finds a client by username
        private ClientSocket FindClientByUsername(string username)
        {
            return clientSockets.FirstOrDefault(c =>
                c.username != null &&
                c.username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        // Sends a message to all clients (optionally excluding sender)
        public void SendToAll(string str, ClientSocket from)
        {
            foreach (ClientSocket c in clientSockets)
            {
                if (from == null || !from.socket.Equals(c))
                {
                    byte[] data = Encoding.ASCII.GetBytes(str);
                    c.socket.Send(data);
                }
            }
        }
    }
}
