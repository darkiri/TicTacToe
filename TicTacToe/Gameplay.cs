using System;
using System.Linq;

namespace TicTacToe
{
    public class Gameplay : IGameplay
    {
        private BoardState _board;
        private readonly PlayStrategy _ai;
        private bool _playWithComputer;
        private BoardMark _whoGoesNow;

        public BoardMark WhoGoesNow
        {
            get { return _whoGoesNow; }
        }

        public BoardState Board
        {
            get { return _board; }
        }

        public Gameplay(PlayStrategy ai)
        {
            _ai = ai;
            _ai.SetMark(BoardMark.O);
            _whoGoesNow = BoardMark.X;
            _board = new BoardState();
        }

        public BoardMark WhoWins()
        {
            return Board.HasCompleteLine(BoardMark.X)
                       ? BoardMark.X
                       : Board.HasCompleteLine(BoardMark.O)
                             ? BoardMark.O
                             : BoardMark._;
        }

        public void Setup(bool playWithComputer)
        {
            _playWithComputer = playWithComputer;
            OnChanged();
        }

        public void Reset()
        {
            _whoGoesNow = BoardMark.X;
            _board = new BoardState();
            OnChanged();
        }

        private void OnChanged()
        {
            if (null != Changed)
            {
                Changed(this, new EventArgs());
            }
        }

        public event EventHandler Changed;

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
                _whoGoesNow = WhoGoesNow.OpponentsMark();
            }
        }
    }
}