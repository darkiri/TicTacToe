using System;
using Moq;
using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class GameControllerTests
    {
        private GameController _controller;
        private Mock<IGameplay> _gameMock;
        private Mock<IView> _viewMock;

        [SetUp]
        public void SetUp()
        {
            _gameMock = new Mock<IGameplay>
                        {
                            DefaultValue = DefaultValue.Mock
                        };
            _viewMock = new Mock<IView>();
            _controller = new GameController(_viewMock.Object, _gameMock.Object);
        }


        [Test]
        public void When_GameplayIsChanged_Then_PlayStateIsRendered()
        {
            _gameMock.Raise(g => g.Changed += null, new EventArgs());
            _viewMock.Verify(v => v.Render(ControllerState.Play.ToString(_gameMock.Object)));
        }
    }
}