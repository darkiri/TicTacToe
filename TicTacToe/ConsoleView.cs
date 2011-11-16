using System;

namespace TicTacToe
{
    internal class ConsoleView : IView
    {
        public void Render(string status)
        {
            Console.Out.WriteLine("");
            Console.Out.WriteLine(status);
        }

        public string GetUserInput()
        {
            return Console.ReadKey().KeyChar.ToString();
        }
    }
}