namespace TicTacToe
{
    public abstract class PlayStrategy
    {
        protected BoardMark OwnMark { get; set; }
        protected BoardMark OpponentsMark { get { return OwnMark.OpponentsMark(); } }

        public void SetMark(BoardMark mark)
        {
            OwnMark = mark;
        }

        public abstract int GetNextPosition(BoardState board);
    }
}