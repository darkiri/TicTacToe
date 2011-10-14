using System;
using System.Linq;

namespace TicTacToe
{
    public class Gameplay : IGameplay
    {
        private BoardState _board;
        private readonly IPlayStrategy _ai;
        private bool _playWithComputer;

        public Gameplay(IPlayStrategy ai)
        {
            _ai = ai;
            _ai.SetMark(BoardMark.O);
            Reset();
        }

        public void Setup(bool playWithComputer)
        {
            _playWithComputer = playWithComputer;
        }

        public BoardMark WhoGoesNow { get; private set; }

        public BoardState Board
        {
            get { return _board; }
        }

        public event EventHandler Changed;

        public BoardMark WhoWins()
        {
            return Board.HasCompleteLine()
                       ? Board.HasCompleteLine(BoardMark.X)
                             ? BoardMark.X
                             : BoardMark.O
                       : BoardMark._;
        }

        public void Reset()
        {
            WhoGoesNow = BoardMark.X;
            _board = new BoardState();
        }

        public void GoTo(int position)
        {
            SetAMark(position);
            OnChanged();

            if (_playWithComputer)
            {
                SetAMark(_ai.GetNextPosition(_board));
                OnChanged();
            }
        }

        private void SetAMark(int position)
        {
            if (!Board.FreePositions.Any(p => p == position))
            {
                throw new InvalidOperationException(String.Format("Cannot go to position {0}!", position));
            } 
            else
            {
                _board = _board.Set(position, WhoGoesNow);
                WhoGoesNow = WhoGoesNow == BoardMark.X ? BoardMark.O : BoardMark.X;
            }
        }

        private void OnChanged()
        {
            if (null != Changed)
            {
                Changed(this, new EventArgs());
            }
        }
    }
}