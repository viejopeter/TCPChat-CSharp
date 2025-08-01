# Windows Forms Chat

A C# Windows Forms Application that allows users to chat over TCP (client-server model). The app supports multiple commands, private messaging (whisper), user moderation, and username management.

# Features

- **Client-Server TCP Chat**
  - Host or join a chat using IP and port
  - Realtime messaging
  - Command-based interactions
- **Username & User List**
  - Set and change your username
  - List all connected users
- **Moderator Controls**
  - Promote/Demote moderators
  - Kick users
- **Private Messaging**
  - Whisper to another user
- **Fun**
  - Tell a joke `!joke`
  - Ask for server time `!time`

## Setup Instructions

1. **Clone this repository**

   ```bash
   git clone https://github.com/viejopeter/TCPChat-CSharp.git
   cd TCPChat-CSharp

    Open in Visual Studio

        Open the .sln file

    Build and Run

        Press F5 or click on Start to run the application

## How to Use
    To Host a Server

    Enter a port number in My Port

    Click Host

    Share your IP and port with others to let them join

## To Join a Server

    Enter your local port in My Port

    Enter server's IP and port

    Click Join

    Optionally change your username and start chatting

## Available Commands

Command	Description

!about	Information about the server
!commands	Lists all available commands
!username <name>	Change your username
!who	Lists all connected users
!whoami	Shows your current username
!whisper <user> <msg>	Sends a private message to another user
!clear	Clears your chat window
!joke	Tells a random joke
!time	Gets the current server time
!help	Shows help for all commands
!kick <user>	(Moderator only) Disconnect a user
!mod <user>	Promote/Demote a user as moderator (Server only)
!mods	List all moderators (Server only)
!exit	Disconnect from the chat