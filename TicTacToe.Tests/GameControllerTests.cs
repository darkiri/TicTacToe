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
        public void When_XHasItsTurn_Then_StatusIsXGoes()
        {
            _gameMock.Setup(g => g.XGoesNow).Returns(true);
            _controller.DoUserInteraction();
            Assert.That(_controller.GetStatus(), Is.EqualTo("X goes:"));
        }

        [Test]
        public void When_OHasItsTurn_Then_StatusIsOGoes()
        {
            _gameMock.Setup(g => g.XGoesNow).Returns(false);
            Assert.That(_controller.GetStatus(), Is.EqualTo("O goes:"));
        }

        [Test]
        public void When_XWins_Then_StatusIsXWins()
        {
            _gameMock.Setup(g => g.XWins()).Returns(true);
            Assert.That(_controller.GetStatus(), Is.EqualTo("X wins!"));
        }

        [Test]
        public void When_OWins_Then_StatusIsOWins()
        {
            _gameMock.Setup(g => g.OWins()).Returns(true);
            Assert.That(_controller.GetStatus(), Is.EqualTo("O wins!"));
        }

        [Test]
        public void When_UserInputsNumber_Then_GameGoesToThePosition()
        {
            _viewMock.Setup(v => v.GetUserInput()).Returns("3");
            _controller.DoUserInteraction();
            _gameMock.Verify(g => g.GoTo(3));
        }

        [Test]
        public void When_UserPresseR_Then_GameIsRestarted()
        {
            _viewMock.Setup(v => v.GetUserInput()).Returns("R");
            _controller.DoUserInteraction();
            _gameMock.Verify(g => g.Reset());
        }

        [Test]
        public void When_UserPresseQ_Then_GameIsExited() 
        {
            _viewMock.Setup(v => v.GetUserInput()).Returns("Q");
            _controller.DoUserInteraction();
            Assert.That(_controller.QuitGame, Is.True);
        }

        [Test]
        public void When_GameplayIsChanged_Then_CurrentStateIsRendered()
        {
            _gameMock.Raise(g => g.Changed += null, new EventArgs());
            _viewMock.Verify(v => v.Render(_gameMock.Object.Board));
            _viewMock.Verify(v => v.Render(_controller.GetStatus()));
        }
    }
}