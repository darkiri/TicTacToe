using System;

namespace TicTacToe
{
    public interface IGameplay
    {
        bool XGoesNow { get; }
        IBoard Board { get; }
        event EventHandler Changed;
        bool XWins();
        bool OWins();
        void Reset();
        void GoTo(int position);
    }
}