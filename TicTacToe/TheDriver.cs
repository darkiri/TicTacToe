namespace TicTacToe
{
    public class TheDriver
    {
        public static void Main()
        {
            StartPipe(InjectComponents());
        }

        private static GameController InjectComponents() 
        {
            return new GameController(new ConsoleView(), new Game(new Board()));
        }

        private static void StartPipe(GameController controller)
        {
            while (!controller.QuitGame) 
            {
                controller.DoUserInteraction();
            }
        }        
    }
}