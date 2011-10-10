using System;
using Moq;
using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class GameplayTests
    {
        private Gameplay _gameplay;
        private Mock<IBoard> _boardMock;

        [SetUp]
        public void SetUp()
        {
            _boardMock = new Mock<IBoard>();
            _boardMock.Setup(b => b.FreePositions).Returns(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
            _gameplay = new Gameplay(_boardMock.Object);
        }

        [Test]
        public void When_XGoesCorner_Then_XIsSetOnTheBoard()
        {
            _gameplay.Setup(true);
            _gameplay.GoTo(0);
            _boardMock.Verify(b => b.Set(0, BoardMark.X));
        }

        [Test]
        public void When_OGoesCenter_Then_OIsSetOnTheBoard()
        {
            _gameplay.Setup(false);
            _gameplay.GoTo(4);
            _boardMock.Verify(b => b.Set(4, BoardMark.O));
        }

        [Test]
        public void When_XGoes_and_OGoes_Then_XandOareSetOnTheBoard()
        {
            _gameplay.Setup(true);
            _gameplay.GoTo(2);
            _gameplay.GoTo(5);
            _boardMock.Verify(b => b.Set(2, BoardMark.X), Times.Once());
            _boardMock.Verify(b => b.Set(5, BoardMark.O), Times.Once());
        }

        [Test]
        public void When_XGoes_Then_GameChangesIsTriggered()
        {
            var eventTriggered = false;
            _gameplay.Changed += (_, __) => eventTriggered = true;
            _gameplay.GoTo(0);
            Assert.True(eventTriggered);
            
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void When_CornerIsNotFree_Then_CannotGoCorner()
        {
            _boardMock.Setup(b => b.FreePositions).Returns(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            _gameplay.GoTo(0);
        }

        [Test]
        public void When_BoardHasXInCorner_and_OInCenter_Then_NobodyWins()
        {
            _boardMock.Setup(b => b.Get(BoardMark.X)).Returns(new[] { 0 });
            _boardMock.Setup(b => b.Get(BoardMark.O)).Returns(new[] { 4 });
            Assert.That(_gameplay.XWins(), Is.False);
            Assert.That(_gameplay.OWins(), Is.False);
        }

        [Test]
        public void When_GameResetted_Then_XGoesNowAndBoardResetted()
        {
            _gameplay.GoTo(0);
            _gameplay.Reset();
            Assert.True(_gameplay.XGoesNow);
            _boardMock.Verify(b=>b.Reset());
        }
    }
}