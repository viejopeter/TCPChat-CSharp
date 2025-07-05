﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;
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
            chatTextBox.Text += "Setting up server...\n";
            serverSocket.Bind(new IPEndPoint(IPAddress.Any, port));
            serverSocket.Listen(0);
            //kick off thread to read connecting clients, when one connects, it'll call out AcceptCallback function
            serverSocket.BeginAccept(AcceptCallback, this);
            chatTextBox.Text += "Server setup complete\n";
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

            AddToChat(text);

            if (text.ToLower() == "!commands") // Client requested time
            {
                byte[] data = Encoding.ASCII.GetBytes("Commands are !commands !about !who !whisper !exit");
                currentClientSocket.socket.Send(data);
                AddToChat("Commands sent to client");
            }
            else if (text.Contains("!username "))
            {
                string requestedUsername = text.Substring(10).Trim();
                bool existUsername = clientSockets.Exists(allClients => allClients.username != null && allClients.username.Equals(requestedUsername, StringComparison.OrdinalIgnoreCase));

                if (existUsername)
                {
                    byte[] rejection = Encoding.ASCII.GetBytes("Username already taken");
                    currentClientSocket.socket.Send(rejection);
                    // Always Shutdown before closing
                    currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                    currentClientSocket.socket.Close();
                    clientSockets.Remove(currentClientSocket);
                    AddToChat("Client disconnected due to duplicate username");
                    return;
                }
                else
                {
                    currentClientSocket.username = requestedUsername;
                    byte[] data = Encoding.ASCII.GetBytes("Username accepted");
                    currentClientSocket.socket.Send(data);
                    AddToChat($"User '{requestedUsername}' joined the chat.");
                }
            }
            else if (text.ToLower() == "!exit") // Client wants to exit gracefully
            {
                // Always Shutdown before closing
                currentClientSocket.socket.Shutdown(SocketShutdown.Both);
                currentClientSocket.socket.Close();
                clientSockets.Remove(currentClientSocket);
                AddToChat("Client disconnected");
                return;
            }
            else
            {
                string displayUsername = currentClientSocket.username ?? $"User-{currentClientSocket.ShortId}";
                string formattedMessage = $"{displayUsername}: {text}";
                //normal message broadcast out to all clients
                SendToAll(formattedMessage, currentClientSocket);
            }
            //we just received a message from this socket, better keep an ear out with another thread for the next one
            currentClientSocket.socket.BeginReceive(currentClientSocket.buffer, 0, ClientSocket.BUFFER_SIZE, SocketFlags.None, ReceiveCallback, currentClientSocket);
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
