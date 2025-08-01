using System;
using System.Data.SQLite;
using System.IO;

namespace Windows_Forms_Chat
{
    public class DatabaseManager
    {
        // This is the connection to the SQLite database
        private SQLiteConnection connection;

        // This is the name of the database file
        private string databasePath = "tic_chat_game.db";

        public DatabaseManager()
        {
            // If the database file does not exist, create it
            if (!File.Exists(databasePath))
            {
                SQLiteConnection.CreateFile(databasePath);
            }

            // Open a connection to the database
            connection = new SQLiteConnection($"Data Source={databasePath};Version=3;");
            connection.Open();

            // Create the Users table if it does not already exist
            CreateUsersTable();
        }

        // This method creates the Users table
        private void CreateUsersTable()
        {
            string createTableQuery = @"
            CREATE TABLE IF NOT EXISTS Users (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                username TEXT UNIQUE,
                password TEXT,
                wins INTEGER DEFAULT 0,
                losses INTEGER DEFAULT 0,
                draws INTEGER DEFAULT 0
            );";

            // Run the SQL command
            using (SQLiteCommand command = new SQLiteCommand(createTableQuery, connection))
            {
                command.ExecuteNonQuery();
            }
        }

        // This method closes the connection to the database
        public void Close()
        {
            connection?.Close();
        }
    }
}
