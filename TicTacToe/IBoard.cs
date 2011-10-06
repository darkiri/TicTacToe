using System.Collections.Generic;

namespace TicTacToe
{
    public interface IBoard
    {
        void SetX(int position);
        void SetO(int position);
        IEnumerable<int> FreePositions { get; }
        IEnumerable<int> XPositions { get; }
        IEnumerable<int> OPositions { get; }
        void Reset();
    }
}