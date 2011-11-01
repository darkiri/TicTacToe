namespace TicTacToe
{
    public abstract class PlayStrategy
    {
        protected BoardMark MyMark { get; set; }

        public void SetMark(BoardMark mark)
        {
            MyMark = mark;
        }

        public abstract int GetNextPosition(BoardState board);
    }
}