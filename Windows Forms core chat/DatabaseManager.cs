using System;
using System.Data.SQLite;
using System.IO;
using System.Reflection.Metadata;

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
                draws INTEGER DEFAULT 0,
                is_authenticated INTEGER NOT NULL DEFAULT 0
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

                // Update is_authenticated to 1
                string updateQuery = "UPDATE Users SET is_authenticated = 1 WHERE username = @username";
                using (SQLiteCommand updateCmd = new SQLiteCommand(updateQuery, connection))
                {
                    updateCmd.Parameters.AddWithValue("@username", username);
                    updateCmd.ExecuteNonQuery();
                }

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

            // Insert the new user into the database with is_authenticated set to 1 (automatically logged in)
            string insertQuery = @"
            INSERT INTO Users (username, password, is_authenticated)
            VALUES (@username, @password, 1);";
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

        // Method to log out a user (sets is_authenticated to 0)
        public void LogoutUser(string username)
        {
            string updateQuery = "UPDATE Users SET is_authenticated = 0 WHERE username = @username";
            // Create and execute the SQL command using the provided username
            using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                // Execute the update command
                command.ExecuteNonQuery();
            }
        }

        // Method to check if a user is currently authenticated (logged in)
        public bool IsUserAuthenticated(string username)
        {
            string query = "SELECT is_authenticated FROM Users WHERE username = @username";
            // Create and execute the SQL command using the provided username
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                // Add the username parameter to the command
                command.Parameters.AddWithValue("@username", username);
                // Execute the query and get the result
                var result = command.ExecuteScalar();
                // Return true if the result is not null and equals 1 (meaning authenticated), otherwise false
                return result != null && Convert.ToInt32(result) == 1;
            }
        }

        // Method to log out all users by resetting their is_authenticated field to 0
        public void LogoutAllUsers()
        {
            // SQL query to update all rows in the Users table and set is_authenticated to 0
            string updateQuery = "UPDATE Users SET is_authenticated = 0";

            // Create and execute the SQL command
            using (SQLiteCommand command = new SQLiteCommand(updateQuery, connection))
            {
                // Execute the update command
                command.ExecuteNonQuery(); // This logs out all users
            }
        }
    }
}
