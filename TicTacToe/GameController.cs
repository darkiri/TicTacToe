using System;

namespace TicTacToe
{
    public class GameController
    {
        private readonly IView _view;
        private readonly IGameplay _gameplay;

        public GameController(IView view, IGameplay gameplay)
        {
            _view = view;
            _gameplay = gameplay;
            _gameplay.Changed += (s, a) => UpdateView(ControllerState.Play);
        }

        private void UpdateView(ControllerState state)
        {
            _view.Render(state.ToString(_gameplay));
        }

        private string RequestUserInput()
        {
            return _view.GetUserInput().ToUpper();
        }

        public ControllerState DoUserInteraction(ControllerState state)
        {
            try
            {
                if (state != ControllerState.Play)
                {
                    UpdateView(state);
                }
                return state.Handle(RequestUserInput(), _gameplay);
            }
            catch (Exception e)
            {
                _view.Render(e.Message);
                return state;
            }
        }
    }
}