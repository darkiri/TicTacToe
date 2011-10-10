using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Gameplay : IGameplay
    {
        private readonly IBoard _board;
        private bool _xGoesNow;

        public Gameplay(IBoard board)
        {
            _board = board;
            _xGoesNow = true;
        }

        public bool XGoesNow
        {
            get { return _xGoesNow; }
        }

        public IBoard Board
        {
            get { return _board; }
        }

        public event EventHandler Changed;

        public void XGoesTo(int position)
        {
            SetAMark(position, true);
        }

        public void OGoesTo(int position)
        {
            SetAMark(position, false);
        }

        private void SetAMark(int position, bool iAmX)
        {
            if (!iAmX && XGoesNow || iAmX && !XGoesNow)
            {
                throw new InvalidOperationException("Not your turn!");
            }
            else
            {
                if (!Board.FreePositions.Any(p => p == position))
                {
                    throw new InvalidOperationException(String.Format("Cannot go to position {0}!", position));
                }
                else
                {
                    _board.Set(position, iAmX ? BoardMark.X : BoardMark.O);
                    _xGoesNow = !iAmX;
                }
            }
        }

        public void Setup(bool xGoesFirst)
        {
            _xGoesNow = xGoesFirst;
        }

        public bool XWins()
        {
            return Board.HasCompleteLine(BoardMark.X);
        }

        public bool OWins()
        {
            return Board.HasCompleteLine(BoardMark.O);
        }

        public void Reset()
        {
            _xGoesNow = true;
            _board.Reset();
        }

        public void GoTo(int position) 
        {
            if (XGoesNow) 
            {
                XGoesTo(position);
            } 
            else 
            {
                OGoesTo(position);
            }
            OnChanged();
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