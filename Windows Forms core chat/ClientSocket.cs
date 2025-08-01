using System;
using System.Net.Sockets;

namespace Windows_Forms_Chat
{
    // Enumeration defining different possible states a client can be in
    public enum ClientState
    {
        // Login state: Client either login or signup step
        Login,

        // Chatting state: Client is in the chat room
        Chatting,

        // Playing state: Client is in the game
        Playing
    }

    public class ClientSocket
    {
        //add other attributes to this, e.g username, what state the client is in etc
        public Socket socket;
        // Size of the buffer used for receiving data
        public const int BUFFER_SIZE = 2048;
        // Buffer for incoming data
        public byte[] buffer = new byte[BUFFER_SIZE];
        // Username of the client
        public string username { get; set; }
        // Unique identifier for each client
        public Guid Id { get; } = Guid.NewGuid();

        // Short ID for display (first 6 chars of Guid, uppercased, no dashes)
        public string ShortId => Id.ToString("N").Substring(0, 6).ToUpper();

        // Moderator status
        public bool IsModerator { get; set; } = false;
        
        // ClientState property to track the client's current state (Login, Chatting, Playing)
        public ClientState State { get; set; } = ClientState.Login;
    }
}
