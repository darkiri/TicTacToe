using System;
using System.Diagnostics;
using System.Linq;

namespace TicTacToe
{
    public class Bob : IPlayStrategy
    {
        private BoardMark _myMark;
        public void SetMark(BoardMark mark)
        {
            _myMark = mark;
        }

        public int GetNextPosition(BoardState board)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var decisionsRoot = new BobDecisionTree(board, _myMark);
            var position = GetBestDecision(decisionsRoot).Position;
            stopwatch.Stop();

            Devinfo("Tree Size {0} built in {1} ms", decisionsRoot.TreeSize, stopwatch.ElapsedMilliseconds);

            return position;
        }

        private  BobDecisionTree GetBestDecision(BobDecisionTree decisionsRoot)
        {
            if (decisionsRoot.Moves.Count() == 0)
            {
                throw new InvalidOperationException("Cannot go anywhere!");
            }
            else
            {
                return decisionsRoot.Moves.OrderByDescending(m=>m.GetWinFactor(_myMark)).First();
            }
        }

        private void Devinfo(string message, params object[] args)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Out.WriteLine(String.Format(message, args));
            Console.ForegroundColor = color;
        }
    }
}