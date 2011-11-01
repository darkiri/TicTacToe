using System.Collections.Generic;
using System.Linq;

namespace TicTacToe {
    public class BobDecisionTree {
        private readonly BoardState _board;
        private readonly int _position;
        private readonly int _decisionsCount;
        private readonly IEnumerable<BobDecisionTree> _children;
        private readonly IEnumerable<BobDecisionTree> _endsFromHere;
        private readonly int _treeSize;

        public BobDecisionTree(BoardState board, BoardMark whosTurn) : this(board, whosTurn, -1, 0) {}

        private BobDecisionTree(BoardState board, BoardMark whosTurn, int position, int decisionsCount)
        {
            _board = board;
            _position = position;
            _decisionsCount = decisionsCount;

            _children = board.HasCompleteLine()
                            ? Enumerable.Empty<BobDecisionTree>()
                            : AllDecisionsFromHere(board, whosTurn, decisionsCount);
            _endsFromHere = IsEndDecision
                                ? new[] {this}
                                : CollectEndDecisions();
            _treeSize = _children.Select(ch => ch.TreeSize).Sum() + 1;
        }

        private static IEnumerable<BobDecisionTree> AllDecisionsFromHere(BoardState board, BoardMark whosTurn, int decisionsCount)
        {
            var positions2Go = new BoardSymmetry(board).GetFreeUniquePositions();
            if (CanOptimizeTreeBuilding(positions2Go, decisionsCount))
            {
                positions2Go = positions2Go.AsParallel();
            }

            return positions2Go
                .Select(p => new BobDecisionTree(board.Set(p, whosTurn), whosTurn.OpponentsMark(), p, decisionsCount + 1))
                .ToArray();
        }

        private static bool CanOptimizeTreeBuilding(IEnumerable<int> positions, int decisionsCount)
        {
            return decisionsCount < 2 && positions.Count() > 2;
        }

        private IEnumerable<BobDecisionTree> CollectEndDecisions() {
            return _children.Count() == 1
                       ? _children
                       : _children.Where(c => c.IsEndDecision).Union(_children.SelectMany(ch => ch._endsFromHere)).ToArray();
        }

        public IEnumerable<BobDecisionTree> Moves {
            get { return _children; }
        }

        private bool IsEndDecision {
            get { return _children.Count() == 0; }
        }

        private IEnumerable<BobDecisionTree> WinEndsFor(BoardMark player) {
            return _endsFromHere.Where(d => d._board.HasCompleteLine(player));
        }

        public int Position {
            get { return _position; }
        }

        public int TreeSize {
            get { return _treeSize; }
        }

        public double GetWinFactor(BoardMark whosTurn)
        {
            return WinForecast(whosTurn, 0) - WinForecast(whosTurn.OpponentsMark(), 1);
        }

        private double WinForecast(BoardMark whosTurn, int movesToGetTurn) 
        {
            return WinEndsFor(whosTurn).Select(e => e.Weight(movesToGetTurn)).Sum();
        }

        private double Weight(int relativeDecisionsCount)
        {
            return 1.0/(_decisionsCount - relativeDecisionsCount -.99);
        }
    }
}