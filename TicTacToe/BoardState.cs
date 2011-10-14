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

        public BoardState()
        {
            _board = Enumerable.Repeat(BoardMark._, 9);
        }

        public BoardState(IEnumerable<BoardMark> marks)
        {
            _board = marks;
        }

        public IEnumerable<int> FreePositions
        {
            get { return GetPositions(BoardMark._); }
        }

        public IEnumerable<int> GetPositions(BoardMark mark)
        {
            return _board
                .Select((m, p) => new {m, p})
                .Where(b => b.m == mark)
                .Select(b => b.p);
        }

        public BoardState Set(int position, BoardMark mark)
        {
            if (_board.ElementAt(position) != BoardMark._)
            {
                throw new InvalidOperationException();
            } else
            {
                return new BoardState(_board.Select((m, p) => p == position ? mark : m));
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

        public IEnumerable<int> GetFreeUniquePositions()
        {
            var diagonal1Mirroring = GetUniquePositions(c => new Tuple<int, int>(c.Item2, c.Item1));
            var diagonal2Mirroring = GetUniquePositions(c => new Tuple<int, int>(2 - c.Item2, 2 - c.Item1));
            var horizontalMirroring = GetUniquePositions(c => new Tuple<int, int>(2 - c.Item1, c.Item2));
            var verticalMirroring = GetUniquePositions(c => new Tuple<int, int>(c.Item1, 2 - c.Item2));

            return diagonal1Mirroring
                .Intersect(diagonal2Mirroring)
                .Intersect(horizontalMirroring)
                .Intersect(verticalMirroring)
                .Where(p => _board.ElementAt(p) == BoardMark._)
                .ToArray();
        }

        private IEnumerable<int> GetUniquePositions(Func<Tuple<int, int>, Tuple<int, int>> mappingFunc)
        {
            var res = new List<int>();
            foreach (var coordinate in Enumerable.Range(0, 9))
            {
                var mirroredCoordinate = ApplySymmetry(coordinate, mappingFunc);
                if (_board.ElementAt(coordinate) == _board.ElementAt(mirroredCoordinate) && !res.Contains(mirroredCoordinate))
                {
                    res.Add(coordinate);
                }
            }
            return res.Count == 6 ? res : FreePositions;
        }

        private static int AsLinear(Tuple<int, int> coordinate)
        {
            return coordinate.Item1*3 + coordinate.Item2%3;
        }

        private static int ApplySymmetry(int position, Func<Tuple<int, int>, Tuple<int, int>> mappingFunc)
        {
            var coordinate = AsCartesian(position);
            return AsLinear(mappingFunc(coordinate));
        }

        private static Tuple<int, int> AsCartesian(int position)
        {
            return new Tuple<int, int>(position/3, position%3);
        }
    }

    public enum BoardMark
    {
        _,
        X,
        O,
    }
}