using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Board : IBoard
    {
        private static readonly int[][] _winPositions =
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

        private readonly BoardMark[] _board;

        public Board()
        {
            _board = new BoardMark[9];
            Init();
        }

        private void Init()
        {
            for (var pos = 0; pos < 9; pos++)
            {
                _board[pos] = BoardMark._;
            }
        }

        public IEnumerable<int> FreePositions
        {
            get { return Get(BoardMark._); }
        }

        public void Reset()
        {
           Init();
        }

        public IEnumerable<int> Get(BoardMark mark)
        {
            return _board
                .Select((m, p) => new { m, p })
                .Where(b => b.m == mark)
                .Select(b => b.p);
        }

        public void Set(int position, BoardMark mark)
        {
            if (_board[position] != BoardMark._)
            {
                throw new InvalidOperationException();
            }
            else
            {
                _board[position] = mark;
            }
        }

        public bool HasCompleteLine(BoardMark mark)
        {
            return _winPositions.Any(line => CompleteLine(line, mark));
        }

        private bool CompleteLine(IEnumerable<int> winLine, BoardMark mark)
        {
            return winLine.All(Get(mark).Contains);
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
                .Aggregate("", (a, m) => a + "|" + (m == BoardMark._ ? " " : m.ToString()))
                .Trim('|');
        }
    }

    public enum BoardMark
    {
        _,
        X,
        O,
    }
}