namespace TicTacToe
{
    public interface IView
    {
        void Render(BoardState board);
        void Render(string status);
        string GetUserInput();
    }
}