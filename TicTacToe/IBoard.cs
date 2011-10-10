using System.Collections.Generic;

namespace TicTacToe
{
    public interface IBoard
    {
        void Set(int position, BoardMark mark);
        IEnumerable<int> FreePositions { get; }
        void Reset();
        bool HasCompleteLine(BoardMark mark);
        IEnumerable<int> Get(BoardMark mark);
    }
}