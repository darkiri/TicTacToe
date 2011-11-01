using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Steve : PlayStrategy
    {
        public override int GetNextPosition(BoardState board)
        {
            int? position = null;
            position = position
                .Try(() => board.GetPositionsToCompleteLine(MyMark).Count() > 0,
                     () => board.GetPositionsToCompleteLine(MyMark).First())
                .Try(() => board.GetPositionsToCompleteLine(MyMark.OpponentsMark()).Count() > 0,
                     () => board.GetPositionsToCompleteLine(MyMark.OpponentsMark()).First())
                .Try(() => GetPositionsForFork(board, MyMark).Count() > 0,
                     () => GetPositionsForFork(board, MyMark).First())
                .Try(() => GetPositionsForFork(board, MyMark.OpponentsMark()).Count() > 0,
                     () => GetPositionsForFork(board, MyMark.OpponentsMark()).First())
                .Try(() => board.FreePositions.Contains(BoardState.Center),
                     () => BoardState.Center)
                .Try(() => board.FreePositions.Any(p => BoardState.Corners.Contains(p)),
                     () => GetBestCorner(board))
                .Try(() => board.FreePositions.Count() > 0,
                     () => board.FreePositions.First());
            if (null == position)
            {
                throw new InvalidOperationException("Cannot go anywhere!");
            }
            else
            {
                return position.Value;
            }
        }

        private IEnumerable<int> GetPositionsForFork(BoardState board, BoardMark mark)
        {
            return board.FreePositions
                .Where(p => board.Set(p, mark).GetPositionsToCompleteLine(mark).Count() > 1);
        }

        private int GetBestCorner(BoardState board)
        {
            var corners = GetFreeCorners(board)
                .Where(p => board.Set(p, MyMark).GetPositionsToCompleteLine(MyMark).Count() > 0);
            return corners.Count() > 0
                       ? corners.First()
                       : GetFreeCorners(board).First();
        }

        private static IEnumerable<int> GetFreeCorners(BoardState board)
        {
            return board.FreePositions.Where(p => BoardState.Corners.Contains(p));
        }
    }

    public static class HandyExtensions
    {
        public static int? Try(this int? position, Func<bool> predicate, Func<int> evaluator)
        {
            return position == null && predicate()
                       ? evaluator()
                       : position;
        }
    }
}