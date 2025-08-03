using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    public class TCPChatServer : TCPChatBase
    {

        private DatabaseManager db = new DatabaseManager();
        public Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        public List<ClientSocket> clientSockets = new List<ClientSocket>();// List of connected clients
        string lsConnectedUsers = "";

        // Factory method to create server instance with a port and UI text box
        public static TCPChatServer createInstance(int port, TextBox chatTextBox)
        {
            TCPChatServer tcp = null;
            //setup if port within range and valid chat box given
            if (port > 0 && port < 65535 && chatTextBox != null)
            {
                tcp = new TCPChatServer();
                tcp.port = port;
                tcp.chatTextBox = chatTextBox;

            }

            //return empty if user not enter useful details
            return tcp;
        }

        // Initializes and starts the TCP server
        public void SetupServer()
        {
            chatTextBox.Text += "Setting up server..." + Environment.NewLine;
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            serverSocket.Listen(0);// Start listening for connections
            serverSocket.BeginAccept(AcceptCallback, this);// Accept first client
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


            serverSocket.BeginAccept(AcceptCallback, null);// Continue accepting more clients
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
                //Update the database to log out the user
                db.LogoutUser(currentClientSocket.username);
                AddToChat("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                currentClientSocket.socket.Close();
                clientSockets.Remove(currentClientSocket);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(currentClientSocket.buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);
            string[] parts = text.Trim().Split(' ', 2);
            // Ensure the command is not empty
            string cmd = parts[0].ToLower();
            // Extract the parameter if it exists
            string targetUsername = parts.Length > 1 ? parts[1].Trim() : null;

            string messageLogServer = $"[Server Log] User '{currentClientSocket.username}'";

            if (cmd == "!commands")
            {
                // Send the information to the client
                byte[] data = Encoding.ASCII.GetBytes("Commands are !commands !about !who !whoami !username !whisper !help !clear !kick !exit ");
                currentClientSocket.socket.Send(data);
                // Log in server UI
                AddToChat($"{messageLogServer} requested the list of commands.");
            }
            else if (cmd == "!username")
            {

                bool existUsername = clientSockets.Any(otherClient =>
                                        otherClient != currentClientSocket &&
                                        otherClient.username.Equals(targetUsername, StringComparison.OrdinalIgnoreCase));

                if (existUsername)
                {
                    // Send the information to the client
                    byte[] rejection = Encoding.ASCII.GetBytes("Username already taken");
                    currentClientSocket.socket.Send(rejection);

                    //Update the database to log out the user
                    db.LogoutUser(currentClientSocket.username);
                    // Always Shutdown before closing
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    // Log in server UI
                    AddToChat($"{messageLogServer} was disconnected for trying to use an existing username: '{targetUsername}'");
                    //normal message broadcast out to all clients
                    SendToAll($"User {currentClientSocket.username} was disconnected", currentClientSocket);
                    return;
                }
                else
                {
                    if (currentClientSocket.username == targetUsername)
                    {
                        // Send the information to the client
                        byte[] sameNameMessage = Encoding.ASCII.GetBytes("You're already using this username.");
                        currentClientSocket.socket.Send(sameNameMessage);
                    }
                    else
                    {
                        oldUsername = currentClientSocket.username;
                        currentClientSocket.username = targetUsername;
                        formattedMessage = $"User '{oldUsername}' changed username to '{targetUsername}' successfully";

                        // Send the information to the client
                        byte[] data = Encoding.ASCII.GetBytes("Username accepted successfully");
                        currentClientSocket.socket.Send(data);
                        // Log in server UI
                        AddToChat(formattedMessage);
                        //normal message broadcast out to all clients
                        SendToAll(formattedMessage, currentClientSocket);
                    }
                }
            }
            else if (cmd == "!who") // Send back a list connected user (usernames)
            {
                lsConnectedUsers = GetConnectedUsersList();

                // Send the list back to the client who asked
                byte[] data = Encoding.ASCII.GetBytes("Connected users:" + Environment.NewLine + lsConnectedUsers);
                currentClientSocket.socket.Send(data);

                // Log in server UI
                AddToChat($"{messageLogServer} used !who command");
            }
            else if (cmd == "!about")// Client requested information about the server
            {
                // Information about the server
                string aboutMessage = "Server Information: " + Environment.NewLine;
                aboutMessage += "Editor: Pedro Q " + Environment.NewLine;
                aboutMessage += "Purpose: TCP Chat server for connecting users and sharing messages " + Environment.NewLine;
                aboutMessage += "Year of Development: 2025 " + Environment.NewLine;

                // Send the information to the client
                byte[] data = Encoding.ASCII.GetBytes(aboutMessage);
                currentClientSocket.socket.Send(data);

                // Log in server UI
                AddToChat($"{messageLogServer} used !about command");
            }
            else if (cmd == "!whoami")
            {
                string currentUsername = currentClientSocket.username;
                string reply = $"Your current username is: {currentUsername}";
                byte[] data = Encoding.ASCII.GetBytes(reply);
                currentClientSocket.socket.Send(data);

                // Log in server UI
                AddToChat($"{messageLogServer} used !whoami command");
            }
            else if (cmd == "!whisper")
            {

                // Split into username and message
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
                        // Send the information to the client
                        byte[] selfWhisperMsg = Encoding.ASCII.GetBytes("You cannot whisper to yourself.");
                        currentClientSocket.socket.Send(selfWhisperMsg);
                        // Log in server UI
                        AddToChat($"{messageLogServer} tried to whisper to themselves.");
                    }
                    else
                    {
                        // Find the target client by username
                        ClientSocket targetClient = FindClientByUsername(whisper_targetUsername);

                        if (targetClient != null)
                        {
                            string whisperMessage = $"[Whisper from {currentClientSocket.username}]: {messageToSend}";

                            // Send the information to target client
                            byte[] data = Encoding.ASCII.GetBytes(whisperMessage);
                            targetClient.socket.Send(data);

                            // Send the information to the client
                            byte[] confirmation = Encoding.ASCII.GetBytes($"[Whisper to {whisper_targetUsername}]: {messageToSend}");
                            currentClientSocket.socket.Send(confirmation);

                            // Log in server UI
                            AddToChat($"{messageLogServer} whispered to {whisper_targetUsername}: {messageToSend}");
                        }
                        else
                        {
                            // Send the information to the client
                            byte[] notFoundMsg = Encoding.ASCII.GetBytes($"User '{whisper_targetUsername}' not found.");
                            currentClientSocket.socket.Send(notFoundMsg);

                            // Log in server UI
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

                // Send the information to the client
                currentClientSocket.socket.Send(Encoding.ASCII.GetBytes(helpText));

                // Log in server UI
                AddToChat($"{messageLogServer} used !help command");
            }
            else if (cmd == "!clear")
            {
                // Send a special signal to the client so it knows to clear the chat
                currentClientSocket.socket.Send(Encoding.ASCII.GetBytes("!clear_chat"));

                // Log on server UI
                AddToChat($"{messageLogServer} used !clear command");
            }
            else if (cmd == "!kick")
            {
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
                        // Always Shutdown before closing
                        targetUser.socket.Send(Encoding.ASCII.GetBytes("[Server Log] You have been kicked by a moderator."));
                        // Log on server UI
                        targetUser.socket.Shutdown(SocketShutdown.Both);
                        // Close the socket
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

                // Log in server UI
                AddToChat($"{messageLogServer} used !joke command");
                SendToAll(message, null);
            }
            else if (cmd == "!exit") // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                //Update the database to log out the user
                db.LogoutUser(currentClientSocket.username);
                currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                currentClientSocket.socket.Close();
                clientSockets.Remove(currentClientSocket);

                // Log on server UI
                AddToChat($"{messageLogServer} used !exit command and has been disconnected");
                //normal message broadcast out to all clients
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
            else if (cmd == "!login")
            {
                // Example: targetUsername = "username=Pedro;state=Chatting"
                if (!string.IsNullOrWhiteSpace(targetUsername))
                {
                    // Split by ';' to get key-value pairs
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
                                // Try to parse the state string to the enum
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
            else
            {
                formattedMessage = $"{currentClientSocket.username}: {text}";
                // Log on server UI
                AddToChat(formattedMessage);
                //normal message broadcast out to all clients
                SendToAll(formattedMessage, currentClientSocket);
            }
            //we just received a message from this socket, better keep an ear out with another thread for the next one
            currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
        }
        // Handles server-side admin commands
        public void HandleServerCommand(string command)
        {
            string[] parts = command.Trim().Split(' ', 2);
            // Ensure the command is not empty
            string cmd = parts[0].ToLower();
            // Extract the parameter if it exists
            string targetUsername = parts.Length > 1 ? parts[1].Trim() : null;

            if (cmd == "!mod")
            {
                ClientSocket targetUser = FindClientByUsername(targetUsername);

                if (targetUser != null)
                {
                    // If the user is found, toggle their moderator status
                    if (targetUser.IsModerator)
                    {
                        // Demote the user from moderator
                        targetUser.IsModerator = false;
                        // Send a message to all clients about the demotion
                        SendToAll($"[Server] {targetUsername} has been demoted from moderator.", null);
                        AddToChat($"[Server Log] Demoted {targetUsername} from moderator.");
                    }
                    else
                    {
                        // Promote the user to moderator
                        targetUser.IsModerator = true;
                        // Send a message to all clients about the promotion
                        SendToAll($"[Server] {targetUsername} has been promoted to moderator.", null);
                        AddToChat($"[Server Log] Promoted {targetUsername} to moderator.");
                    }
                }
                else
                {
                    // If the user is not found, log it and return
                    AddToChat($"[Server] User '{targetUsername}' not found.");

                }
            }
            else if (cmd == "!mods")
            {
                // List all current moderators
                var mods = clientSockets.Where(c => c.IsModerator).Select(c => c.username).ToList();
                if (mods.Count == 0)
                {
                    // If no moderators are found, log it in the server side
                    AddToChat("[Server Log] No moderators currently assigned.");
                }
                else
                {
                    // If moderators are found, list them
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
                // Handle unknown commands for server
                AddToChat("[Server Log] Unknown server command.");
            }
        }
        // Returns all connected usernames
        public string GetConnectedUsersList()
        {
            // Check if there are any connected clients
            if (clientSockets.Count == 0)
                return "No users connected";

            // StringBuilder is used for efficient string concatenation
            StringBuilder sb = new StringBuilder();

            // Loop through each connected client
            foreach (ClientSocket c in clientSockets)
            {
                // Only add the username if it's not null or empty
                if (!string.IsNullOrEmpty(c.username))
                    sb.AppendLine(c.username);
            }

            // If no valid usernames were added, return fallback message
            if (string.IsNullOrWhiteSpace(sb.ToString()))
                return "No users connected";

            // Return the list of connected usernames, removing trailing newlines
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
