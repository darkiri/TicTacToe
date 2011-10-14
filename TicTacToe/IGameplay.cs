using System;

namespace TicTacToe
{
    public interface IGameplay
    {
        BoardMark WhoGoesNow { get; }
        BoardState Board { get; }
        event EventHandler Changed;
        BoardMark WhoWins();
        void Reset();
        void GoTo(int position);
        void Setup(bool playWithComputer);
    }
}