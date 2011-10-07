using Moq;
using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class GameControllerTests
    {
        private GameController _controller;
        private Mock<IGame> _gameMock;
        private Mock<IView> _viewMock;

        [SetUp]
        public void SetUp()
        {
            _gameMock = new Mock<IGame>
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
            _viewMock.Verify(v => v.Render("X goes:"));
        }

        [Test]
        public void When_OHasItsTurn_Then_StatusIsOGoes()
        {
            _gameMock.Setup(g => g.XGoesNow).Returns(false);
            _controller.DoUserInteraction();
            _viewMock.Verify(v => v.Render("O goes:"));
        }

        [Test]
        public void When_XWins_Then_StatusIsXWins()
        {
            _gameMock.Setup(g => g.XWins()).Returns(true);
            _controller.DoUserInteraction();
            _viewMock.Verify(v => v.Render("X wins!"));
        }

        [Test]
        public void When_OWins_Then_StatusIsOWins()
        {
            _gameMock.Setup(g => g.OWins()).Returns(true);
            _controller.DoUserInteraction();
            _viewMock.Verify(v => v.Render("O wins!"));
        }

        [Test]
        public void When_XHasItsTurn_and_PositionIsSelected_Then_XMarkIsSetOnThePosition()
        {
            _gameMock.Setup(g => g.XGoesNow).Returns(true);
            _viewMock.Setup(v => v.GetUserInput()).Returns("0");
            _controller.DoUserInteraction();
            _gameMock.Verify(g => g.XGoesTo(0));
        }

        [Test]
        public void When_OHasItsTurn_and_PositionIsSelected_Then_OMarkIsSetOnThePosition()
        {
            _gameMock.Setup(g => g.XGoesNow).Returns(false);
            _viewMock.Setup(v => v.GetUserInput()).Returns("1");
            _controller.DoUserInteraction();
            _gameMock.Verify(g => g.OGoesTo(1));
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
        }
    }
}