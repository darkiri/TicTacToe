namespace TicTacToe
{
    public interface IGame
    {
        bool XGoesNow { get; }
        IBoard Board { get; }
        void XGoesTo(int position);
        void OGoesTo(int position);
        bool XWins();
        bool OWins();
        void Reset();
    }
}