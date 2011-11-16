using Moq;
using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class ControllerStateTests
    {
        private Mock<IGameplay> _gameMock;

        [SetUp]
        public void SetUp()
        {
            _gameMock = new Mock<IGameplay>
            {
                DefaultValue = DefaultValue.Mock
            };
        }

        [Test]
        public void When_XHasItsTurn_Then_StatusIsXGoes()
        {
            _gameMock.Setup(g => g.WhoGoesNow).Returns(BoardMark.X);
            AssertPlayStateToString(_gameMock, "X goes:");
        }

        private void AssertPlayStateToString(Mock<IGameplay> gameMock, string value)
        {
            Assert.That(ControllerState.Play.ToString(gameMock.Object).Trim(), Is.EqualTo(value));
        }

        [Test]
        public void When_OHasItsTurn_Then_StatusIsOGoes()
        {
            _gameMock.Setup(g => g.WhoGoesNow).Returns(BoardMark.O);
            AssertPlayStateToString(_gameMock, "O goes:");
        }

        [Test]
        public void When_XWins_Then_StatusIsXWins()
        {
            _gameMock.Setup(g => g.WhoWins()).Returns(BoardMark.X);
            AssertPlayStateToString(_gameMock, "X wins!");
        }

        [Test]
        public void When_OWins_Then_StatusIsOWins()
        {
            _gameMock.Setup(g => g.WhoWins()).Returns(BoardMark.O);
            AssertPlayStateToString(_gameMock, "O wins!");
        }

        [Test]
        public void When_SomeoneWins_Then_TheGameIsOver()
        {
            _gameMock.Setup(g => g.WhoWins()).Returns(BoardMark.O);
            var endState = ControllerState.Play.Handle("1", _gameMock.Object);
            Assert.That(endState, Is.EqualTo(ControllerState.GameOver));
        }

        [Test]
        public void When_NotWinSituation_Then_TheGameCanBePlayed()
        {
            _gameMock.Setup(g => g.WhoWins()).Returns(BoardMark._);
            var endState = ControllerState.Play.Handle("1", _gameMock.Object);
            Assert.That(endState, Is.EqualTo(ControllerState.Play));
        }

        [Test]
        public void When_UserInputsNumber_Then_GameGoesToThePosition()
        {
            ControllerState.Play.Handle("3", _gameMock.Object);
            _gameMock.Verify(g => g.GoTo(3));
        }

        [Test]
        public void When_UserPresseR_Then_GameIsRestarted()
        {
            var nextState = ControllerState.Play.Handle("R", _gameMock.Object);
            Assert.That(nextState, Is.EqualTo(ControllerState.Play));
            _gameMock.Verify(g => g.Reset());
        }

        [Test]
        public void When_UserPresseQ_Then_GameIsExited()
        {
            var endState = ControllerState.Play.Handle("Q", _gameMock.Object);
            Assert.That(endState, Is.EqualTo(ControllerState.Quit));
        }

        [Test]
        public void When_SetupState_Then_TheGameplayShouleBeConfigured()
        {
            ControllerState.Setup.Handle("Y", _gameMock.Object);
            _gameMock.Verify(g => g.Setup(true));
        }

        [Test]
        public void When_SetupGame_and_UserEntersNeitherYNorN_Then_HeShouldBeQuestionedAgain()
        {
            var nextState = ControllerState.Setup.Handle("b", _gameMock.Object);
            Assert.That(nextState, Is.EqualTo(ControllerState.Setup));
        }
    }
}