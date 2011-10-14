using System;
using Moq;
using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class GameplayTests
    {
        private Gameplay _gameplay;
        private Mock<IPlayStrategy> _ai;

        [SetUp]
        public void SetUp()
        {
            _ai = new Mock<IPlayStrategy>();
            _gameplay = new Gameplay(_ai.Object);
        }

        [Test]
        public void When_XGoesCorner_Then_XIsSetOnTheBoard()
        {
            _gameplay.GoTo(0);
            Assert.That(_gameplay.Board.GetPositions(BoardMark.X), Is.EquivalentTo(new[] {0}));
        }

        [Test]
        public void When_XGoes_and_OGoes_Then_XandOareSetOnTheBoard()
        {
            _gameplay.GoTo(2);
            _gameplay.GoTo(5);
            Assert.That(_gameplay.Board.GetPositions(BoardMark.X), Is.EquivalentTo(new[] {2}));
            Assert.That(_gameplay.Board.GetPositions(BoardMark.O), Is.EquivalentTo(new[] {5}));
        }

        [Test]
        public void When_XGoes_Then_GameChangesIsTriggered()
        {
            var numTriggered = 0;
            _gameplay.Changed += (_, __) => numTriggered++;
            _gameplay.GoTo(0);
            Assert.That(numTriggered, Is.EqualTo(1));
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void When_CornerIsNotFree_Then_CannotGoCorner()
        {
            _gameplay.GoTo(0);
            _gameplay.GoTo(0);
        }

        [Test]
        public void When_BoardHasXInCorner_and_OInCenter_Then_NobodyWins()
        {
            _gameplay.GoTo(0);
            _gameplay.GoTo(4);
            Assert.That(_gameplay.WhoWins(), Is.EqualTo(BoardMark._));
        }

        [Test]
        public void When_GameResetted_Then_XGoesNowAndBoardResetted()
        {
            _gameplay.GoTo(0);
            _gameplay.Reset();
            Assert.That(_gameplay.WhoGoesNow, Is.EqualTo(BoardMark.X));
            Assert.That(_gameplay.Board.FreePositions, Is.EquivalentTo(new[] {0, 1, 2, 3, 4, 5, 6, 7, 8}));
        }

        [Test]
        public void When_PlayingWithAComputer_Then_GamplayIsChangedTwiceInAUserTurn()
        {
            _gameplay.Setup(true);

            var numTriggered = 0;
            _gameplay.Changed += (_, __) => numTriggered++;

            _ai.Setup(p => p.GetNextPosition(It.IsAny<BoardState>())).Returns(1);
            _gameplay.GoTo(0);
            Assert.That(numTriggered, Is.EqualTo(2));
        }

        [Test]
        public void When_PlayingWithAComputer_Then_ThePositionIsAutomaticallyCalculated()
        {
            _gameplay.Setup(true);
            _ai.Setup(p => p.GetNextPosition(It.IsAny<BoardState>())).Returns(1);
            _gameplay.GoTo(0);
            _ai.Verify(ai => ai.GetNextPosition(It.IsAny<BoardState>()));
        }

        [Test]
        public void When_WithoutComputerPlayer_Then_ThePositionShouldNeverBeAutomaticallySet()
        {
            _gameplay.Setup(false);
            _gameplay.GoTo(0);
            _ai.Verify(ai => ai.GetNextPosition(_gameplay.Board), Times.Never());
        }
    }
}