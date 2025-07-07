using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows.Forms;

//https://github.com/AbleOpus/NetworkingSamples/blob/master/MultiServer/Program.cs
namespace Windows_Forms_Chat
{
    public class TCPChatServer : TCPChatBase
    {

        public Socket serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //connected clients
        public List<ClientSocket> clientSockets = new List<ClientSocket>();

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

        public void SetupServer()
        {
            chatTextBox.Text += "Setting up server..." + Environment.NewLine;
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            serverSocket.Listen(0);
            //kick off thread to read connecting clients, when one connects, it'll call out AcceptCallback function
            serverSocket.BeginAccept(AcceptCallback, this);
            chatTextBox.Text += "Server setup complete " + Environment.NewLine;
        }



        public void CloseAllSockets()
        {
            foreach (ClientSocket clientSocket in clientSockets)
            {
                clientSocket.socket.Shutdown(SocketShutdown.Both);
                clientSocket.socket.Close();
            }
            clientSockets.Clear();
            serverSocket.Close();
        }

        public void AcceptCallback(IAsyncResult AR)
        {
            Socket joiningSocket;

            try
            {
                joiningSocket = serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            ClientSocket newClientSocket = new ClientSocket();
            newClientSocket.socket = joiningSocket;
            newClientSocket.username = $"User-{newClientSocket.ShortId}";

            clientSockets.Add(newClientSocket);
            //start a thread to listen out for this new joining socket. Therefore there is a thread open for each client
            joiningSocket.BeginReceive(newClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, newClientSocket);
            AddToChat("Client connected, waiting for request...");

            //we finished this accept thread, better kick off another so more people can join
            serverSocket.BeginAccept(AcceptCallback, null);
        }

        public void ReceiveCallback(IAsyncResult AR)
        {
            ClientSocket currentClientSocket = (ClientSocket)AR.AsyncState;

            int received;
            string oldUsername = "";
            string lsConnectedUsers = "";
            string formattedMessage = "";


            try
            {
                received = currentClientSocket.socket.EndReceive(AR);
            }
            catch (SocketException)
            {
                AddToChat("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                currentClientSocket.socket.Close();
                clientSockets.Remove(currentClientSocket);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(currentClientSocket.buffer, recBuf, received);
            string text = Encoding.ASCII.GetString(recBuf);

            string messageLogServer = $"[Server Log] User '{currentClientSocket.username}'";

            if (text.ToLower() == "!commands") // Client requested time
            {
                // Send the information to the client
                byte[] data = Encoding.ASCII.GetBytes("Commands are !commands !about !who !whoami !username !whisper !help !clear !exit ");
                currentClientSocket.socket.Send(data);
                // Log in server UI
                AddToChat($"{messageLogServer} requested the list of commands.");
            }
            else if (text.Contains("!username "))
            {
                string requestedUsername = text.Substring(10).Trim();
                bool existUsername = clientSockets.Any(otherClient =>
                                        otherClient != currentClientSocket &&
                                        otherClient.username.Equals(requestedUsername, StringComparison.OrdinalIgnoreCase));

                if (existUsername)
                {
                    // Send the information to the client
                    byte[] rejection = Encoding.ASCII.GetBytes("Username already taken");
                    currentClientSocket.socket.Send(rejection);
                    // Always Shutdown before closing
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    // Log in server UI
                    AddToChat($"{messageLogServer} was disconnected for trying to use an existing username: '{requestedUsername}'");
                    //normal message broadcast out to all clients
                    SendToAll($"User {currentClientSocket.username} was disconnected", currentClientSocket);
                    return;
                }
                else
                {
                    if(currentClientSocket.username == requestedUsername)
                    {
                        // Send the information to the client
                        byte[] sameNameMessage = Encoding.ASCII.GetBytes("You're already using this username.");
                        currentClientSocket.socket.Send(sameNameMessage);
                    }
                    else
                    {
                        oldUsername = currentClientSocket.username;
                        currentClientSocket.username = requestedUsername;
                        formattedMessage = $"User '{oldUsername}' changed username to '{requestedUsername}' successfully";

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
            else if (text.ToLower() == "!who") // Send back a list connected user (usernames)
            {
                foreach(ClientSocket c in clientSockets)
                {
                    lsConnectedUsers += c.username + Environment.NewLine;
                }

                // Send the list back to the client who asked
                byte[] data = Encoding.ASCII.GetBytes("Connected users:" + Environment.NewLine + lsConnectedUsers);
                currentClientSocket.socket.Send(data);

                // Log in server UI
                AddToChat($"{messageLogServer} used !who command");
            }
            else if (text.ToLower() == "!about") // Client requested information about the server
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
            else if (text.ToLower() == "!whoami")
            {
                string currentUsername = currentClientSocket.username;
                string reply = $"Your current username is: {currentUsername}";
                byte[] data = Encoding.ASCII.GetBytes(reply);
                currentClientSocket.socket.Send(data);

                // Log in server UI
                AddToChat($"{messageLogServer} used !whoami command");
            }
            else if (text.ToLower().StartsWith("!whisper "))
            {
                // Extract the full message without the command
                string commandContent = text.Substring(9).Trim();

                // Split into username and message
                int spaceIndex = commandContent.IndexOf(' ');
                if (spaceIndex == -1)
                {
                    byte[] errorMsg = Encoding.ASCII.GetBytes("Invalid whisper format. Use: !whisper [username] [message]");
                    currentClientSocket.socket.Send(errorMsg);
                }
                else
                {
                    string targetUsername = commandContent.Substring(0, spaceIndex);
                    string messageToSend = commandContent.Substring(spaceIndex + 1);

                    

                    if (currentClientSocket.username != null &&
                        currentClientSocket.username.Equals(targetUsername, StringComparison.OrdinalIgnoreCase))
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
                        ClientSocket targetClient = FindClientByUsername(targetUsername);

                        if (targetClient != null)
                        {
                            string whisperMessage = $"[Whisper from {currentClientSocket.username}]: {messageToSend}";

                            // Send the information to target client
                            byte[] data = Encoding.ASCII.GetBytes(whisperMessage);
                            targetClient.socket.Send(data);

                            // Send the information to the client
                            byte[] confirmation = Encoding.ASCII.GetBytes($"[Whisper to {targetUsername}]: {messageToSend}");
                            currentClientSocket.socket.Send(confirmation);
                            
                            // Log in server UI
                            AddToChat($"{messageLogServer} whispered to {targetUsername}: {messageToSend}");
                        }
                        else
                        {
                            // Send the information to the client
                            byte[] notFoundMsg = Encoding.ASCII.GetBytes($"User '{targetUsername}' not found.");
                            currentClientSocket.socket.Send(notFoundMsg);

                            // Log in server UI
                            AddToChat($"{messageLogServer} tried to whisper to non existent user '{targetUsername}'");

                        }
                    }
                }
            }
            else if (text.ToLower() == "!help")
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
                    "!exit = Disconnect from chat";
                
                // Send the information to the client
                currentClientSocket.socket.Send(Encoding.ASCII.GetBytes(helpText));
                
                // Log in server UI
                AddToChat($"{messageLogServer} used !help command");
            }
            else if (text.ToLower() == "!clear")
            {
                // Send a special signal to the client so it knows to clear the chat
                currentClientSocket.socket.Send(Encoding.ASCII.GetBytes("!clear_chat"));

                // Log on server UI
                AddToChat($"{messageLogServer} used !clear command");
            }
            else if (text.StartsWith("!kick "))
            {
                if (!currentClientSocket.IsModerator)
                {
                    currentClientSocket.socket.Send(Encoding.ASCII.GetBytes("[Server Log] Only moderators can kick users."));
                    AddToChat($"{messageLogServer} attempted to use the !kick command but is not a moderator.");
                }
                else
                {
                    string targetUsername = text.Substring(6).Trim();

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
            else if (text.ToLower() == "!exit") // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                currentClientSocket.socket.Close();
                clientSockets.Remove(currentClientSocket);

                // Log on server UI
                AddToChat($"{messageLogServer} used !exit command and has been disconnected");
                //normal message broadcast out to all clients
                SendToAll($"User {currentClientSocket.username} was disconnected", currentClientSocket);
                return;
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
            else
            {
                // Handle unknown commands for server
                AddToChat("[Server Log] Unknown server command.");
            }
        }

        private ClientSocket FindClientByUsername(string username)
        {
            return clientSockets.FirstOrDefault(c =>
                c.username != null &&
                c.username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

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
