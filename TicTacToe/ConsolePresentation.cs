using System;

namespace TicTacToe
{
    public class ConsolePresentation
    {
        public static void Main()
        {
            StartPipe(InjectComponents());
        }

        private static void StartPipe(GameController controller)
        {
            for (;;)
            {
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");
                Console.Out.WriteLine(controller.Board.ToString());
                Console.Out.WriteLine("");
                Console.Out.WriteLine(controller.Status);

                var key = Console.ReadKey();
                
                Console.Out.WriteLine("");
                Console.Out.WriteLine("");

                if (key.KeyChar == 'Q' || key.KeyChar == 'q')
                {
                    break;
                }
                else if (key.KeyChar == 'R' || key.KeyChar == 'r')
                {
                    controller.Restart();
                }
                else if (Char.IsDigit(key.KeyChar))
                {
                    try
                    {
                        controller.GoTo(Convert.ToInt32(key.KeyChar.ToString()));
                    }
                    catch (Exception e)
                    {
                        Console.Out.WriteLine(e.Message);
                    }
                }
                else
                {
                    Console.Out.WriteLine("Enter position (0-9)");
                }
            }
        }

        private static GameController InjectComponents()
        {
            return new GameController(new Game(new Board()));
        }
    }
}