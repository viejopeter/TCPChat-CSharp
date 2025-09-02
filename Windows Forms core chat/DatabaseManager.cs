using System;
using System.Collections.Generic;
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

        // Increments the 'wins' count for the specified user in the Users table
        public void AddWin(string username)
        {
            string query = "UPDATE Users SET wins = wins + 1 WHERE username = @username";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery(); // Executes the update query
            }
        }

        // Increments the 'losses' count for the specified user in the Users table
        public void AddLoss(string username)
        {
            string query = "UPDATE Users SET losses = losses + 1 WHERE username = @username";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery(); // Executes the update query
            }
        }

        // Increments the 'draws' count for the specified user in the Users table
        public void AddDraw(string username)
        {
            string query = "UPDATE Users SET draws = draws + 1 WHERE username = @username";
            using (SQLiteCommand command = new SQLiteCommand(query, connection))
            {
                command.Parameters.AddWithValue("@username", username);
                command.ExecuteNonQuery(); // Executes the update query
            }
        }

        // Retrieves all user scores sorted by: highest wins first, then highest draws, then lowest losses
        public List<(string Username, int Wins, int Draws, int Losses)> GetAllScoresSorted()
        {
            var scores = new List<(string, int, int, int)>();
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "SELECT username, wins, draws, losses FROM Users ORDER BY wins DESC, draws DESC, losses ASC";
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        string username = reader.GetString(0);
                        int wins = reader.GetInt32(1);
                        int draws = reader.GetInt32(2);
                        int losses = reader.GetInt32(3);
                        scores.Add((username, wins, draws, losses)); // Adds the user's score to the list
                    }
                }
            }
            return scores; // Returns the sorted list of scores
        }

        // Updates a user's username in the Users table
        public void UpdateUsername(string oldUsername, string newUsername)
        {
            using (var cmd = connection.CreateCommand())
            {
                cmd.CommandText = "UPDATE Users SET username = @newUsername WHERE username = @oldUsername";
                cmd.Parameters.AddWithValue("@newUsername", newUsername);
                cmd.Parameters.AddWithValue("@oldUsername", oldUsername);
                cmd.ExecuteNonQuery(); // Executes the update query
            }
        }

        // Checks whether a given username already exists in the Users table
        public bool UsernameExists(string username)
        {
            // Create a new SQL command using the active database connection
            using (var cmd = connection.CreateCommand())
            {
                // SQL query to count how many users have the specified username
                cmd.CommandText = "SELECT COUNT(*) FROM Users WHERE username = @username";
                cmd.Parameters.AddWithValue("@username", username);

                // Execute the query and return true if one or more users match the username
                return Convert.ToInt32(cmd.ExecuteScalar()) > 0;
            }
        }
    }
}
