using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Game : IGame
    {
        private readonly IBoard _board;
        private bool _xGoesNow;

        private readonly int[][] _winPositions =
            new[]
                {
                    new[] {0, 1, 2},
                    new[] {3, 4, 5},
                    new[] {6, 7, 8},
                    new[] {0, 3, 6},
                    new[] {1, 4, 7},
                    new[] {2, 5, 8},
                    new[] {0, 4, 8},
                    new[] {2, 4, 6}
                };

        public Game(IBoard board)
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

        public void XGoesTo(int position)
        {
            SetAMark(Board.SetX, position, true);
        }

        public void OGoesTo(int position)
        {
            SetAMark(Board.SetO, position, false);
        }

        private void SetAMark(Action<int> setPosition, int position, bool iAmX)
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
                    setPosition(position);
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
            return _winPositions.Any(line => CompleteLine(line, Board.XPositions));
        }

        private bool CompleteLine(IEnumerable<int> winLine, IEnumerable<int> positions)
        {
            return winLine.All(positions.Contains);
        }

        public bool OWins()
        {
            return _winPositions.Any(line => CompleteLine(line, Board.OPositions));
        }

        public void Reset()
        {
            _xGoesNow = true;
            _board.Reset();
        }
    }
}