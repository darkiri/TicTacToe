using System;
using System.Collections.Generic;
using System.Linq;

namespace TicTacToe
{
    public class Steve : PlayStrategy
    {
        private BoardState _board;

        public override int GetNextPosition(BoardState board)
        {
            _board = board;
            return EmptyPosition
                .Try(IfICanWin,
                     GoToWinPosition)
                .Try(IfOpponentCanWin,
                     BlockWinPosition)
                .Try(IfICanFork,
                     GoToForkPosition)
                .Try(IfOpponentCanFork,
                     BlockForkPosition)
                .Try(IfCenterFree,
                     () => BoardState.Center)
                .Try(IfAnyCornerFree,
                     GetBestCorner)
                .Try(() => board.FreePositions.Count() > 0,
                     () => board.FreePositions.First())
                .Return();
        }

        private static int? EmptyPosition
        {
            get { return null; }
        }

        private bool IfICanWin()
        {
            return _board.GetPositionsToCompleteLine(OwnMark).Count() > 0;
        }

        private bool IfOpponentCanWin()
        {
            return _board.GetPositionsToCompleteLine(OpponentsMark).Count() > 0;
        }

        private int GoToWinPosition()
        {
            return _board.GetPositionsToCompleteLine(OwnMark).First();
        }

        private int BlockWinPosition()
        {
            return _board.GetPositionsToCompleteLine(OpponentsMark).First();
        }

        private bool IfICanFork()
        {
            return GetForkPositions(OwnMark).Count() > 0;
        }

        private bool IfOpponentCanFork()
        {
            return GetForkPositions(OpponentsMark).Count() > 0;
        }

        private int GoToForkPosition()
        {
            return GetForkPositions(OwnMark).First();
        }

        private int BlockForkPosition()
        {
            return GetForkPositions(OpponentsMark).First();
        }

        private IEnumerable<int> GetForkPositions(BoardMark mark)
        {
            return _board.FreePositions
                .Where(p => _board.Set(p, mark).GetPositionsToCompleteLine(mark).Count() > 1);
        }

        private bool IfCenterFree()
        {
            return _board.FreePositions.Contains(BoardState.Center);
        }

        private bool IfAnyCornerFree()
        {
            return _board.FreePositions.Any(p => BoardState.Corners.Contains(p));
        }

        private int GetBestCorner()
        {
            var winCorners = GetFreeCorners()
                .Where(p => _board.Set(p, OwnMark).GetPositionsToCompleteLine(OwnMark).Count() > 0)
                .ToArray();
            return winCorners.Count() > 0
                       ? winCorners.First()
                       : GetFreeCorners().First();
        }

        private IEnumerable<int> GetFreeCorners()
        {
            return _board.FreePositions.Where(p => BoardState.Corners.Contains(p));
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

        public static int Return(this int? position)
        {
            if (null == position)
            {
                throw new InvalidOperationException("Cannot go anywhere!");
            }
            else
            {
                return position.Value;
            }
        }
    }
}