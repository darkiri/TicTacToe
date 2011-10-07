using System;

namespace TicTacToe {
    internal class ConsoleView : IView {
        public void Render(IBoard board) {
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");
            Console.Out.WriteLine(board.ToString());
        }

        public void Render(string status) {
            Console.Out.WriteLine("");
            Console.Out.WriteLine(status);
            Console.Out.WriteLine("");
            Console.Out.WriteLine("");

        }

        public string GetUserInput() {
            return Console.ReadKey().KeyChar.ToString();
        }
    }
}