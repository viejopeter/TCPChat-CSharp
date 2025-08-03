using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Windows_Forms_Chat
{
    public enum TileType
    {
        blank, cross, naught
    }
    public enum GameState
    {
        playing, draw, crossWins, naughtWins
    }

    public class TicTacToe
    {
        //TODO change myTurn to false and playerTileType to blank for defaults
        //they should be dictated by the server
        public bool myTurn = true;
        public TileType playerTileType = TileType.cross;
        public List<Button> buttons = new List<Button>();//assuming 9 in order
        public TileType[] grid = new TileType[9];

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
        public void StringToGrid(string s)
        {
            //TODO take string s e.g "xox___x_o" and use its values to update grid and the buttons
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
                if (buttons.Count >= 9)
                    buttons[i].Text = TileTypeToString(grid[i]);
            }
        }

        public bool SetTile(int index, TileType tileType)
        {
            if(grid[index] == TileType.blank)
            {
                grid[index] = tileType;
                if (buttons.Count >= 9)
                    buttons[index].Text = TileTypeToString(tileType);
                return true;
            }
            //else

            return true;

        }

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
        public bool CheckForWin(TileType t)
        {
            //horizontals
            if (grid[0] == t && grid[1] == t && grid[2] == t)
                return true;
            if (grid[3] == t && grid[4] == t && grid[5] == t)
                return true;
            if (grid[6] == t && grid[7] == t && grid[8] == t)
                return true;

            //verticals
            if (grid[0] == t && grid[3] == t && grid[6] == t)
                return true;
            if (grid[1] == t && grid[4] == t && grid[7] == t)
                return true;
            if (grid[2] == t && grid[5] == t && grid[8] == t)
                return true;

            //diagonals
            if (grid[0] == t && grid[4] == t && grid[8] == t)
                return true;
            if (grid[2] == t && grid[4] == t && grid[6] == t)
                return true;


            //nothing
            return false;
        }

        public bool CheckForDraw()
        {
            for(int i = 0; i < 9; i++)
            {
                if (grid[i] == TileType.blank)
                    return false;
            }

            return true;
        }

        public void ResetBoard()
        {
            for (int i = 0; i < 9; i++)
            {
                grid[i] = TileType.blank;
                if (buttons.Count >= 9)
                    buttons[i].Text = TileTypeToString(TileType.blank);
            }
        }

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
