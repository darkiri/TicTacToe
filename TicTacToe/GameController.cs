using System;

namespace TicTacToe 
{
    public class GameController 
    {
        private readonly IView _view;
        private readonly IGameplay _gameplay;

        public GameController(IView view, IGameplay gameplay) 
        {
            _view = view;
            _gameplay = gameplay;
            _gameplay.Changed += (s, a) => UpdateView();
            UpdateView();
        }

        private void UpdateView()
        {
            _view.Render(_gameplay.Board);
            _view.Render(GetStatus());
        }

        public string GetStatus() 
        {
            return _gameplay.XWins()
                       ? "X wins!"
                       : _gameplay.OWins()
                             ? "O wins!"
                             : _gameplay.XGoesNow
                                   ? "X goes:"
                                   : "O goes:";
        }

        public bool QuitGame { get; private set; }

        public void DoUserInteraction() 
        {            
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
            switch (key)
            {
                case "Q":
                    QuitGame = true;
                    break;
                case "R":
                    _gameplay.Reset();
                    break;
                default:
                    _gameplay.GoTo(ParsePosition(key));
                    break;
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
    }
}