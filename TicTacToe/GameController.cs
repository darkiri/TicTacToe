using System;

namespace TicTacToe 
{
    public class GameController 
    {
        private readonly IView _view;
        private readonly IGame _game;

        public GameController(IView view, IGame game) 
        {
            _view = view;
            _game = game;
        }

        private string GetStatus() 
        {
            return _game.XWins()
                       ? "X wins!"
                       : _game.OWins()
                             ? "O wins!"
                             : _game.XGoesNow
                                   ? "X goes:"
                                   : "O goes:";
        }

        public bool QuitGame { get; private set; }

        public void DoUserInteraction() 
        {            
            _view.Render(_game.Board);
            _view.Render(GetStatus());

            try 
            {
                HandleInput(_view.GetUserInput().ToUpper());
            } 
            catch (Exception e) 
            {
                _view.Render(e.Message);
            }            
        }

        private void HandleInput(string key) 
        {
            if (key == "Q") 
            {
                QuitGame = true;
            } 
            else if (key == "R") 
            {
                _game.Reset();
            } 
            else
            {
                GoTo(ParsePosition(key));               
            }            
        }

        private static int ParsePosition(string key) 
        {            
            if (key.Length == 1 && Char.IsDigit(key[0])) 
            {
                return Convert.ToInt32(key);
            } 
            else 
            {
                throw new FormatException("Enter position (0-8)");
            }
        }

        private void GoTo(int position) 
        {
            if (_game.XGoesNow) 
            {
                _game.XGoesTo(position);
            } 
            else 
            {
                _game.OGoesTo(position);
            }
        }
    }
}