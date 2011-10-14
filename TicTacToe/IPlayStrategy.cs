namespace TicTacToe
{
    public interface IPlayStrategy
    {
        int GetNextPosition(BoardState board);
        void SetMark(BoardMark mark);
    }
}