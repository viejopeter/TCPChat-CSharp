# Windows Forms Chat

A Windows Forms application for real-time chat and multiplayer Tic-Tac-Toe, featuring user authentication, persistent score tracking, and a rich set of chat commands. Built with C# (.NET 8), SQLite, and asynchronous TCP networking.

# Features

## User Authentication
- Secure sign-up and login with password validation  
- Unique usernames enforced  
- Authentication status tracked in the database  

## Chat System
- Public chat room for all connected users  
- Private messaging (`!whisper [username] [message]`)  
- Moderator commands (`!kick`, `!mod`, `!mods`)  
- Server and client-side command support (`!who`, `!about`, `!help`, `!clear`, `!exit`, etc.)  

## Tic-Tac-Toe Game
- Integrated multiplayer game  
- Join with `!join` command; assigned as player 1 or 2  
- Server manages game state, turn order, and board updates  
- Board state broadcast to all clients for real-time viewing  
- Game results (win, lose, draw, disconnect) handled and scores updated  

## Leaderboard
- Persistent score tracking (wins, draws, losses) in SQLite  
- View sorted leaderboard with `!scores` command  

## Robust Networking
- Asynchronous TCP sockets for scalable client-server communication  
- Graceful handling of disconnects and errors  

## Setup Instructions

1. **Clone this repository**

   ```bash
   git clone https://github.com/viejopeter/TCPChat-CSharp.git
   cd TCPChat-CSharp

    Open in Visual Studio

        Open the .sln file

    Build and Run

        Press F5 or click on Start to run the application

## To Join a Server

    Enter your local port in My Port

    Enter server's IP and port

    Click Join

    Optionally change your username and start chatting

# Usage

## Chat Commands
- `!who` — List connected users  
- `!about` — Server info  
- `!help` — List all commands  
- `!username [newName]` — Change your username  
- `!whisper [username] [message]` — Send a private message  
- `!kick [username]` — Moderator: kick a user  
- `!mod [username]` — Promote/demote moderator  
- `!mods` — List moderators  
- `!clear` — Clear chat window  
- `!exit` — Disconnect  

## Game Commands
- `!join` — Join Tic-Tac-Toe game  
- `!move [0-8]` — Make a move (automatically sent by UI)  
- `!scores` — View leaderboard  


# Database

- **SQLite file:** `tic_chat_game.db` (auto-created in project directory)  

## Tables

### Users
- `username` — Unique identifier for each user  
- `password` — Securely stored password  
- `wins` — Number of games won  
- `losses` — Number of games lost  
- `draws` — Number of games drawn  
- `is_authenticated` — Tracks whether the user is logged in  

# License
MIT License  

# Credits
- Developed by **Pedro Q**  
- For **Networking and Database Systems**, Bachelor of Information Technology  


