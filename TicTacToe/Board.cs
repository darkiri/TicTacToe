using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Board : IBoard
    {
        private enum Mark
        {
            _,
            X,
            O,
        }

        private readonly Mark[] _board;

        public Board()
        {
            _board = new Mark[9];
            Init();
        }

        private void Init()
        {
            for (var pos = 0; pos < 9; pos++)
            {
                _board[pos] = Mark._;
            }
        }

        public IEnumerable<int> FreePositions
        {
            get { return GetBoardPositions(Mark._); }
        }

        public IEnumerable<int> XPositions
        {
            get { return GetBoardPositions(Mark.X); }
        }

        public IEnumerable<int> OPositions
        {
            get { return GetBoardPositions(Mark.O); }
        }

        public void Reset()
        {
           Init();
        }

        private IEnumerable<int> GetBoardPositions(Mark mark)
        {
            return _board
                .Select((m, p) => new { m, p })
                .Where(b => b.m == mark)
                .Select(b => b.p);
        }

        public void SetX(int position)
        {
            SetMark(position, Mark.X);
        }

        public void SetO(int position)
        {
            SetMark(position, Mark.O);
        }

        private void SetMark(int position, Mark mark)
        {
            if (_board[position] != Mark._)
            {
                throw new InvalidOperationException();
            }
            else
            {
                _board[position] = mark;
            }
        }

        public override string ToString()
        {
            return DrawLine(0) + Separators() + DrawLine(1) + Separators() + DrawLine(2);
        }

        private static string Separators()
        {
            return "\n\r- - -\n\r";
        }

        private string DrawLine(int line)
        {
            return _board
                .Skip(3*line)
                .Take(3)
                .Aggregate("", (a, m) => a + "|" + (m == Mark._ ? " " : m.ToString()))
                .Trim('|');
        }
    }
}