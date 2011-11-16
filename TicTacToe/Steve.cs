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
                .Try(() => CanWinNow(board, OwnMark),
                     () => GetWinPosition(board, OwnMark))
                .Try(() => CanWinNow(board, OpponentsMark),
                     () => GetWinPosition(board, OpponentsMark))
                .Try(() => GetForkPositions(board, OwnMark).Count() > 0,
                     () => GetForkPositions(board, OwnMark).First())
                .Try(() => GetForkPositions(board, OpponentsMark).Count() > 0,
                     () => GetForkPositions(board, OpponentsMark).First())
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

        private bool CanWinNow(BoardState board, BoardMark mark)
        {
            return board.GetPositionsToCompleteLine(mark).Count() > 0;
        }

        private int GetWinPosition(BoardState board, BoardMark mark)
        {
            return board.GetPositionsToCompleteLine(mark).First();
        }

        private IEnumerable<int> GetForkPositions(BoardState board, BoardMark mark)
        {
            return board.FreePositions
                .Where(p => board.Set(p, mark).GetPositionsToCompleteLine(mark).Count() > 1);
        }

        private int GetBestCorner(BoardState board)
        {
            var corners = GetFreeCorners(board)
                .Where(p => board.Set(p, OwnMark).GetPositionsToCompleteLine(OwnMark).Count() > 0);
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