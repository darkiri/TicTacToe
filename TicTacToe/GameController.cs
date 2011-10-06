namespace TicTacToe
{
    public class GameController
    {
        private readonly IGame _game;

        public GameController(IGame game)
        {
            _game = game;
        }

        public string Status
        {
            get
            {
                return _game.XWins()
                           ? "X wins!"
                           : _game.OWins()
                                 ? "O wins!"
                                 : _game.XGoesNow
                                       ? "X goes:"
                                       : "O goes:";
            }
        }

        public IBoard Board
        {
            get { return _game.Board; }
        }

        public void GoTo(int position)
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

        public void Restart()
        {
            _game.Reset();
        }
    }
}