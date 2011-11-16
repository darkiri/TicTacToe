namespace TicTacToe
{
    public class TheDriver
    {
        private static GameController _controller;

        public static void Main()
        {
            _controller = InjectComponents();
            Pipe(ControllerState.Setup);
        }

        private static GameController InjectComponents()
        {
            return new GameController(new ConsoleView(), new Gameplay(new Steve()));
        }

        private static void Pipe(ControllerState state)
        {
            if (state != ControllerState.Quit)
            {
                Pipe(_controller.DoUserInteraction(state));
            }
        }
    }
}