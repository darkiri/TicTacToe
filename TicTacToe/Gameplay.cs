using System;
using System.Linq;

namespace TicTacToe
{
    public class Gameplay : IGameplay
    {
        private BoardState _board;
        private readonly PlayStrategy _ai;
        private bool _playWithComputer;

        public Gameplay(PlayStrategy ai)
        {
            _ai = ai;
            _ai.SetMark(BoardMark.O);
            Reset();
        }

        public void Setup(bool playWithComputer)
        {
            _playWithComputer = playWithComputer;
            OnChanged();
        }

        public BoardMark WhoGoesNow { get; private set; }

        public BoardState Board
        {
            get { return _board; }
        }

        public event EventHandler Changed;

        public BoardMark WhoWins()
        {
            return Board.HasCompleteLine(BoardMark.X)
                       ? BoardMark.X
                       : Board.HasCompleteLine(BoardMark.O)
                             ? BoardMark.O
                             : BoardMark._;
        }

        public void Reset()
        {
            WhoGoesNow = BoardMark.X;
            _board = new BoardState();
            OnChanged();
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
                WhoGoesNow = WhoGoesNow.OpponentsMark();
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