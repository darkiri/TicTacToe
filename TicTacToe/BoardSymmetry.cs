using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class BoardSymmetry
    {
        private readonly BoardState _boardState;

        public BoardSymmetry(BoardState boardState)
        {
            _boardState = boardState;
        }

        public int[] GetFreeUniquePositions()
        {
            var diagonal1Mirroring = GetUniquePositions(c => new Tuple<int, int>(c.Item2, c.Item1));
            var diagonal2Mirroring = GetUniquePositions(c => new Tuple<int, int>(2 - c.Item2, 2 - c.Item1));
            var horizontalMirroring = GetUniquePositions(c => new Tuple<int, int>(2 - c.Item1, c.Item2));
            var verticalMirroring = GetUniquePositions(c => new Tuple<int, int>(c.Item1, 2 - c.Item2));

            return diagonal1Mirroring
                .Intersect(diagonal2Mirroring)
                .Intersect(horizontalMirroring)
                .Intersect(verticalMirroring)
                .Where(p => _boardState.MarkAt(p) == BoardMark._)
                .ToArray();
        }

        private IEnumerable<int> GetUniquePositions(Func<Tuple<int, int>, Tuple<int, int>> mappingFunc)
        {
            var res = new List<int>();
            foreach (var coordinate in Enumerable.Range(0, 9))
            {
                var mirroredCoordinate = AsLinear(mappingFunc(AsCartesian(coordinate)));
                if (_boardState.MarkAt(coordinate) == _boardState.MarkAt(mirroredCoordinate) && !res.Contains(mirroredCoordinate))
                {
                    res.Add(coordinate);
                }
            }
            return res.Count == 6 ? res : _boardState.FreePositions;
        }

        private static int AsLinear(Tuple<int, int> coordinate)
        {
            return coordinate.Item1*3 + coordinate.Item2%3;
        }

        private static Tuple<int, int> AsCartesian(int position)
        {
            return new Tuple<int, int>(position/3, position%3);
        }
    }
}