using System;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    // Base class for managing chat functionality
    public class TCPChatBase
    {
        // The TextBox control used to display chat messages
        public TextBox chatTextBox;
        // The port used for communication
        public int port;

        // Method to update the chat display with a new message
        public void SetChat(string str)
        {
            chatTextBox.Invoke((Action)delegate
            {
                chatTextBox.Text = str;
                chatTextBox.AppendText(Environment.NewLine);
            });
        }
        // Method to append a new message to the existing chat display
        public void AddToChat(string str)
        {
            chatTextBox.Invoke((Action)delegate
            {
                // Appends the provided string to the existing chat display
                chatTextBox.AppendText(str);
                // Adds a new line after the message
                chatTextBox.AppendText(Environment.NewLine);
            });
        }
    }
}
