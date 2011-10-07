namespace TicTacToe {
    public interface IView {
        void Render(IBoard board);
        void Render(string status);
        string GetUserInput();
    }
}