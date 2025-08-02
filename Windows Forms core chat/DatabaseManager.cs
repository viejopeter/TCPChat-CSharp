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

        // Login method: checks if username and password match
        public string LoginUser(string username, string password)
        {
            string query = "SELECT password FROM Users WHERE username = @username";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                var result = command.ExecuteScalar();

                if (result == null)
                    return "User does not exist.";

                if (result.ToString() != password)
                    return "Incorrect password.";

                return "Login successful.";
            }
        }

        // Sign up method: creates a new user after verifying uniqueness and password match
        public string SignUpUser(string username, string password, string confirmPassword)
        {
            if (password != confirmPassword)
                return "Passwords do not match.";

            string checkQuery = "SELECT COUNT(*) FROM Users WHERE username = @username";
            using (SQLiteCommand checkCmd = new SQLiteCommand(checkQuery, connection))
            {
                checkCmd.Parameters.AddWithValue("@username", username);
                int exists = Convert.ToInt32(checkCmd.ExecuteScalar());
                if (exists > 0)
                    return "Username already exists.";
            }

            string insertQuery = "INSERT INTO Users (username, password) VALUES (@username, @password)";
            using (SQLiteCommand insertCmd = new SQLiteCommand(insertQuery, connection))
            {
                insertCmd.Parameters.AddWithValue("@username", username);
                insertCmd.Parameters.AddWithValue("@password", password);
                insertCmd.ExecuteNonQuery();
            }

            return "Sign up successful.";
        }

        // This method closes the connection to the database
        public void Close()
        {
            connection?.Close();
        }
    }
}
