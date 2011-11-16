namespace TicTacToe
{
    public interface IView
    {
        void Render(string status);
        string GetUserInput();
    }
}