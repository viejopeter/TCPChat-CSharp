using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    // Enum representing the type of tile in the Tic-Tac-Toe grid
    public enum TileType
    {
        blank, // Empty tile
        cross, // Player X
        naught // Player O
    }
    // Enum representing the state of the game
    public enum GameState
    {
        playing,   // Game is ongoing
        draw,      // Game ended in a draw
        crossWins, // Player X wins
        naughtWins // Player O wins
    }

    // Main class for Tic-Tac-Toe game logic and UI integration
    public class TicTacToe
    {
        // Indicates if it's the local player's turn (should be set by server)
        public bool myTurn = true;
        // The tile type assigned to the local player (should be set by server)
        public TileType playerTileType = TileType.cross;
        // List of UI buttons representing the board (should contain 9 buttons)
        public List<Button> buttons = new List<Button>();
        // Array representing the board state
        public TileType[] grid = new TileType[9];

        // Converts the grid to a string for network transmission (e.g. "xox___x_o")
        public string GridToString()
        {
            char[] chars = new char[9];
            for (int i = 0; i < 9; i++)
            {
                switch (grid[i])
                {
                    case TileType.cross:
                        chars[i] = 'x';
                        break;
                    case TileType.naught:
                        chars[i] = 'o';
                        break;
                    default:
                        chars[i] = '_';
                        break;
                }
            }
            return new string(chars);
        }

        // Updates the grid and UI buttons from a string (received from server)
        public void StringToGrid(string s)
        {
            // Example input: "xox___x_o"
            for (int i = 0; i < Math.Min(s.Length, 9); i++)
            {
                switch (s[i])
                {
                    case 'x':
                    case 'X':
                        grid[i] = TileType.cross;
                        break;
                    case 'o':
                    case 'O':
                        grid[i] = TileType.naught;
                        break;
                    default:
                        grid[i] = TileType.blank;
                        break;
                }
                // Update button text if buttons are initialized
                if (buttons.Count >= 9)
                    buttons[i].Text = TileTypeToString(grid[i]);
            }
        }

        // Attempts to set a tile at the given index for the given player
        public bool SetTile(int index, TileType tileType)
        {
            if(grid[index] == TileType.blank)
            {
                grid[index] = tileType;
                if (buttons.Count >= 9)
                    buttons[index].Text = TileTypeToString(tileType);
                return true;
            }
            // If tile is not blank, move is invalid (but returns true for legacy reasons)
            return true;
        }

        // Returns the current state of the game (playing, win, draw)
        public GameState GetGameState()
        {
            GameState state = GameState.playing;
            if (CheckForWin(TileType.cross))
                state = GameState.crossWins;
            else if (CheckForWin(TileType.naught))
                state = GameState.naughtWins;
            else if (CheckForDraw())
                state = GameState.draw;
            return state;
        }

        // Checks if the given tile type has won the game
        public bool CheckForWin(TileType t)
        {
            // Check horizontal lines
            if (grid[0] == t && grid[1] == t && grid[2] == t)
                return true;
            if (grid[3] == t && grid[4] == t && grid[5] == t)
                return true;
            if (grid[6] == t && grid[7] == t && grid[8] == t)
                return true;
            // Check vertical lines
            if (grid[0] == t && grid[3] == t && grid[6] == t)
                return true;
            if (grid[1] == t && grid[4] == t && grid[7] == t)
                return true;
            if (grid[2] == t && grid[5] == t && grid[8] == t)
                return true;
            // Check diagonals
            if (grid[0] == t && grid[4] == t && grid[8] == t)
                return true;
            if (grid[2] == t && grid[4] == t && grid[6] == t)
                return true;
            // No win found
            return false;
        }

        // Checks if the board is full and no winner (draw)
        public bool CheckForDraw()
        {
            for(int i = 0; i < 9; i++)
            {
                if (grid[i] == TileType.blank)
                    return false;
            }
            return true;
        }

        // Resets the board and UI buttons to blank
        public void ResetBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                grid[i] = TileType.blank;
                if (buttons.Count >= 9)
                    buttons[i].Text = TileTypeToString(TileType.blank);
            }
        }

        // Converts a TileType enum to its string representation for UI
        public static string TileTypeToString(TileType t)
        {
            if (t == TileType.blank)
                return "";
            else if (t == TileType.cross)
                return "X";
            else
                return "O";
        }
    }
}
