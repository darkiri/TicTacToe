﻿using System;
using Moq;
using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class GameplayTests
    {
        private Game _game;
        private Mock<IBoard> _boardMock;

        [SetUp]
        public void SetUp()
        {
            _boardMock = new Mock<IBoard>();
            _boardMock.Setup(b => b.FreePositions).Returns(new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8 });
            _game = new Game(_boardMock.Object);
        }

        [Test]
        public void WhenXGoesCorner_ThenXIsSetOnTheBoard()
        {
            _game.XGoesTo(0);
            _boardMock.Verify(b => b.SetX(0));
        }

        [Test]
        public void WhenOGoesCenter_ThenOIsSetOnTheBoard()
        {
            _game.Setup(false);
            _game.OGoesTo(4);
            _boardMock.Verify(b => b.SetO(4));
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void WhenXGoes_ThenXCannotGoAgain()
        {
            _game.XGoesTo(0);
            _game.XGoesTo(1);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void WhenOGoes_ThenOCannotGoAgain()
        {
            _game.Setup(false);
            _game.OGoesTo(0);
            _game.OGoesTo(1);
        }

        [Test]
        public void WhenOGoes_and_XGoes_ThenOCanGoAgain()
        {
            _game.Setup(false);
            _game.OGoesTo(0);
            _game.XGoesTo(1);
            _game.OGoesTo(2);

            _boardMock.Verify(b => b.SetO(0), Times.Once());
            _boardMock.Verify(b => b.SetX(1), Times.Once());
            _boardMock.Verify(b => b.SetO(2), Times.Once());
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void WhenXGoesCorner_ThenOCannotGoCorner()
        {
            _game.XGoesTo(0);
            _boardMock.Setup(b => b.FreePositions).Returns(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            _game.OGoesTo(0);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void WhenOGoesCorner_ThenXCannotGoCorner()
        {
            _game.Setup(false);
            _game.OGoesTo(0);
            _boardMock.Setup(b => b.FreePositions).Returns(new[] { 1, 2, 3, 4, 5, 6, 7, 8 });
            _game.XGoesTo(0);
        }

        [Test]
        public void WhenBoardHasXInCorner_and_OInCenter_ThenNobodyWins()
        {
            _boardMock.Setup(b => b.XPositions).Returns(new[] { 0 });
            _boardMock.Setup(b => b.OPositions).Returns(new[] { 4 });
            Assert.That(_game.XWins(), Is.False);
            Assert.That(_game.OWins(), Is.False);
        }

        [TestCase(new[]{0, 1, 2}, TestName = "X First Line")]
        [TestCase(new[] { 3, 4, 5 }, TestName = "X Second Line")]
        [TestCase(new[] { 6, 7, 8 }, TestName = "X Third Line")]
        [TestCase(new[] { 0, 3, 6 }, TestName = "X First Column")]
        [TestCase(new[] { 1, 4, 7 }, TestName = "X Second Column")]
        [TestCase(new[] { 2, 5, 8 }, TestName = "X Third Column")]
        [TestCase(new[] { 0, 4, 8 }, TestName = "X First Diagonal")]
        [TestCase(new[] { 2, 4, 6 }, TestName = "X Second Diagonal")]
        public void WhenBoardHasFullLineX_ThenXWins(int[] line)
        {
            _boardMock.Setup(b => b.XPositions).Returns(line);
            Assert.That(_game.XWins(), Is.True);
            Assert.That(_game.OWins(), Is.False);
        }

        [TestCase(new[] { 0, 1, 2 }, TestName = "O First Line")]
        [TestCase(new[] { 3, 4, 5 }, TestName = "O Second Line")]
        [TestCase(new[] { 6, 7, 8 }, TestName = "O Third Line")]
        [TestCase(new[] { 0, 3, 6 }, TestName = "O First Column")]
        [TestCase(new[] { 1, 4, 7 }, TestName = "O Second Column")]
        [TestCase(new[] { 2, 5, 8 }, TestName = "O Third Column")]
        [TestCase(new[] { 0, 4, 8 }, TestName = "O First Diagonal")]
        [TestCase(new[] { 2, 4, 6 }, TestName = "O Second Diagonal")]
        public void WhenBoardHasFullLineO_ThenOWins(int[] line)
        {
            _boardMock.Setup(b => b.OPositions).Returns(line);
            Assert.That(_game.XWins(), Is.False);
            Assert.That(_game.OWins(), Is.True);
        }

        [Test]
        public void WhenGameResetted_ThenXGoesNowAndBoardResetted()
        {
            _game.XGoesTo(0);
            _game.Reset();
            Assert.True(_game.XGoesNow);
            _boardMock.Verify(b=>b.Reset());
        }
    }
}