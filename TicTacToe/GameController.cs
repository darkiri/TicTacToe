using System;

namespace TicTacToe
{
    public class GameController
    {
        private readonly IView _view;
        private readonly IGameplay _gameplay;
        private bool _ready2Play;

        public GameController(IView view, IGameplay gameplay)
        {
            _view = view;
            _gameplay = gameplay;
            _gameplay.Changed += (s, a) => UpdateView();
        }

        private void UpdateView()
        {
            _view.Render(_gameplay.Board);
            _view.Render(GetStatus());
        }

        public string GetStatus()
        {
            return _gameplay.WhoWins() == BoardMark._
                       ? String.Format("{0} goes:", _gameplay.WhoGoesNow)
                       : String.Format("{0} wins!", _gameplay.WhoWins());
        }

        public bool QuitGame { get; private set; }

        public void DoUserInteraction()
        {
            try
            {
                if (!_ready2Play)
                {
                    _view.Render("Play with the Computer? (Y/N)");
                }
                HandleInput(RequestUserInput());
            } 
            catch (Exception e)
            {
                _view.Render(e.Message);
            }
        }

        private string RequestUserInput()
        {
            return _view.GetUserInput().ToUpper();
        }

        private void HandleInput(string key)
        {
            switch (key)
            {
                case "Q":
                    QuitGame = true;
                    break;
                case "R":
                    ResetGame();
                    break;
                default:
                    if (_ready2Play)
                    {
                        _gameplay.GoTo(ParsePosition(key));
                    }
                    else
                    {
                        MatchSetupInput(key);
                    }
                    break;
            }
        }

        private void ResetGame()
        {
            _gameplay.Reset();
            UpdateView();
        }

        private void MatchSetupInput(string key)
        {
            switch (key)
            {
                case "Y":
                    SetupGame(true);
                    break;
                case "N":
                    SetupGame(false);
                    break;
            }
        }

        private void SetupGame(bool playWithComputer)
        {
            _gameplay.Setup(playWithComputer);
            _ready2Play = true;
            UpdateView();
        }

        private static int ParsePosition(string key)
        {
            if (key.Length == 1 && Char.IsDigit(key[0]))
            {
                return Convert.ToInt32(key);
            } else
            {
                throw new FormatException("Enter position (0-8)");
            }
        }
    }
}