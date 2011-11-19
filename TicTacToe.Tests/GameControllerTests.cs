using System;
using Machine.Specifications;
using Machine.Specifications.Annotations;
using Moq;
using It = Machine.Specifications.It;

namespace TicTacToe.Tests
{
    public class with_game_controller
    {
        public static Mock<IView> ViewMock;
        public static Mock<IGameplay> GameMock;
        public static GameController Controller;
        public static Mock<ControllerState> StateMock;

        private Establish context = () =>
        {
            GameMock = new Mock<IGameplay>
                        {
                            DefaultValue = DefaultValue.Mock
                        };
            ViewMock = new Mock<IView>();
            Controller = new GameController(ViewMock.Object, GameMock.Object);
            StateMock = new Mock<ControllerState>();

            ViewMock.Setup(v => v.GetUserInput()).Returns("1");
        };
    }

    [Subject(typeof(GameController))]    
    public class when_game_is_changed : with_game_controller
    {
        Because of = () => GameMock.Raise(g => g.Changed += null, new EventArgs());
        It should_render_the_game_state = () => ViewMock.Verify(v => v.Render(ControllerState.Play.ToString(GameMock.Object)));
    }

    [Subject(typeof(GameController))]    
    public class by_user_interaction : with_game_controller
    {
        Because of = () => Controller.DoUserInteraction(StateMock.Object);
        It should_request_user_input = () => ViewMock.Verify(v => v.GetUserInput());
        It should_handle_user_input = () => StateMock.Verify(s => s.Handle("1", GameMock.Object));
    }
}