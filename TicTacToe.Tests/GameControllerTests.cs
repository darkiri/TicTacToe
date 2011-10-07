using System;
using Moq;
using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class GameControllerTests
    {
        private GameController _controller;
        private Mock<IGame> _gameMock;

        [SetUp]
        public void SetUp()
        {
            _gameMock = new Mock<IGame>
                            {
                                DefaultValue = DefaultValue.Mock
                            };
            _controller = new GameController(_gameMock.Object);
        }

        [Test]
        public void When_XHasItsTurn_Then_StatusIsXGoes()
        {
            _gameMock.Setup(g => g.XGoesNow).Returns(true);
            Assert.That(_controller.Status, Is.EqualTo("X goes:"));
        }

        [Test]
        public void When_OHasItsTurn_Then_StatusIsOGoes()
        {
            _gameMock.Setup(g => g.XGoesNow).Returns(false);
            Assert.That(_controller.Status, Is.EqualTo("O goes:"));
        }

        [Test]
        public void When_XWins_Then_StatusIsXWins()
        {
            _gameMock.Setup(g => g.XWins()).Returns(true);
            Assert.That(_controller.Status, Is.EqualTo("X wins!"));
        }

        [Test]
        public void When_OWins_Then_StatusIsOWins()
        {
            _gameMock.Setup(g => g.OWins()).Returns(true);
            Assert.That(_controller.Status, Is.EqualTo("O wins!"));
        }

        [Test]
        public void When_XHasItsTurn_and_PositionIsSelected_Then_XMarkIsSetOnThePosition()
        {
            _gameMock.Setup(g => g.XGoesNow).Returns(true);
            _controller.GoTo(0);
            _gameMock.Verify(g => g.XGoesTo(0));
        }

        [Test]
        public void When_OHasItsTurn_and_PositionIsSelected_Then_OMarkIsSetOnThePosition()
        {
            _gameMock.Setup(g => g.XGoesNow).Returns(false);
            _controller.GoTo(1);
            _gameMock.Verify(g => g.OGoesTo(1));
        }

        [Test]
        public void When_BoardIsRendered_BoardShouldBeRequestedFromGame()
        {
            Console.Out.WriteLine(_controller.Board);
            _gameMock.Verify(g => g.Board);
        }

        [Test]
        public void When_GameRestarted_Then_GameIsResetted()
        {
            _controller.Restart();
            _gameMock.Verify(g => g.Reset());
        }
    }
}