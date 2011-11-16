using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class BoardState
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

        private readonly IEnumerable<BoardMark> _board;
        public static IEnumerable<int> Corners = new[] { 0, 2, 6, 8 };
        public const int Center = 4;

        public BoardState()
        {
            _board = Enumerable.Repeat(BoardMark._, 9);
        }

        public BoardState(IEnumerable<BoardMark> marks)
        {
            _board = marks;
        }

        public IEnumerable<int> GetPositions(BoardMark mark)
        {
            return _board
                .Select((m, p) => new {Mark = m, Position = p})
                .Where(b => b.Mark == mark)
                .Select(b => b.Position);
        }

        public IEnumerable<int> FreePositions
        {
            get { return GetPositions(BoardMark._); }
        }

        public BoardMark MarkAt(int position)
        {
            return _board.ElementAt(position);
        }

        public BoardState Set(int position, BoardMark mark)
        {
            if (_board.ElementAt(position) != BoardMark._)
            {
                throw new InvalidOperationException();
            } else
            {
                return new BoardState(_board.Select((oldMark, p) => p == position ? mark : oldMark));
            }
        }

        public bool HasCompleteLine()
        {
            return HasCompleteLine(BoardMark.X) || HasCompleteLine(BoardMark.O);
        }

        public bool HasCompleteLine(BoardMark mark)
        {
            return _winPositions.Any(line => CompleteLine(line, mark));
        }

        private bool CompleteLine(IEnumerable<int> winLine, BoardMark mark)
        {
            return winLine.All(GetPositions(mark).Contains);
        }

        public IEnumerable<int> GetPositionsToCompleteLine(BoardMark mark)
        {
           return _winPositions
                .Where(line => line.Count(GetPositions(mark).Contains) == 2)
                .SelectMany(line => line)
                .Where(p => _board.ElementAt(p) == BoardMark._);
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

    public static class BoardMarkExtensions
    {
        public static BoardMark OpponentsMark(this BoardMark currentTurn)
        {
            return currentTurn == BoardMark.O ? BoardMark.X : BoardMark.O;
        }
    }
}