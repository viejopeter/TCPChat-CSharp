using System;
using System.Net.Sockets;

namespace Windows_Forms_Chat
{
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
    }
}
