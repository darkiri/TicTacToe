using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class BoardTests
    {
        private static void AssertBoardPositions(IEnumerable<int> actual, params int[] expected)
        {
            Assert.That(actual.ToArray(), Is.EquivalentTo(expected));
        }

        [Test]
        public void When_GameNotStarted_Then_TheBoardIsEmpty()
        {
            var board = new Board();
            AssertBoardPositions(board.FreePositions, 0, 1, 2, 3, 4, 5, 6, 7, 8);
            AssertBoardPositions(board.Get(BoardMark.X));
            AssertBoardPositions(board.Get(BoardMark.O));
        }

        [Test]
        public void When_XGoesCorner_Then_TheBoardIsX00000000()
        {
            var board = new Board();
            board.Set(0, BoardMark.X);
            AssertBoardPositions(board.FreePositions, 1, 2, 3, 4, 5, 6, 7, 8);
            AssertBoardPositions(board.Get(BoardMark.X), 0);
            AssertBoardPositions(board.Get(BoardMark.O));
        }

        [Test]
        public void When_XGoesCorner_and_OGoesSide_Then_TheBoardIsXO0000000()
        {
            var board = new Board();
            board.Set(0, BoardMark.X);
            board.Set(1, BoardMark.O);
            AssertBoardPositions(board.FreePositions, 2, 3, 4, 5, 6, 7, 8);
            AssertBoardPositions(board.Get(BoardMark.X), 0);
            AssertBoardPositions(board.Get(BoardMark.O), 1);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void When_XGoesCenter_Then_OCannotGoCenter()
        {
            var board = new Board();
            board.Set(4, BoardMark.X);
            board.Set(4, BoardMark.O);
        }

        [Test]
        public void When_BoardResetted_Then_AllPositionsFree()
        {
            var board = new Board();
            board.Set(0, BoardMark.X);
            board.Set(1, BoardMark.O);
            board.Reset();
            AssertBoardPositions(board.FreePositions, 0, 1, 2, 3, 4, 5, 6, 7, 8);
        }

        [TestCase(new[] { 0, 1, 2 }, TestName = "X First Line")]
        [TestCase(new[] { 3, 4, 5 }, TestName = "X Second Line")]
        [TestCase(new[] { 6, 7, 8 }, TestName = "X Third Line")]
        [TestCase(new[] { 0, 3, 6 }, TestName = "X First Column")]
        [TestCase(new[] { 1, 4, 7 }, TestName = "X Second Column")]
        [TestCase(new[] { 2, 5, 8 }, TestName = "X Third Column")]
        [TestCase(new[] { 0, 4, 8 }, TestName = "X First Diagonal")]
        [TestCase(new[] { 2, 4, 6 }, TestName = "X Second Diagonal")]
        public void When_BoardHasFullLineX_Then_XWins(int[] line)
        {
            var board = SetUpBoard(line, BoardMark.X);
            Assert.That(board.HasCompleteLine(BoardMark.X), Is.True);
        }

        private static Board SetUpBoard(IEnumerable<int> line, BoardMark mark)
        {
            var board = new Board();
            foreach (var position in line)
            {
                board.Set(position, mark);
            }
            return board;
        }

        [TestCase(new[] { 0, 1, 2 }, TestName = "O First Line")]
        [TestCase(new[] { 3, 4, 5 }, TestName = "O Second Line")]
        [TestCase(new[] { 6, 7, 8 }, TestName = "O Third Line")]
        [TestCase(new[] { 0, 3, 6 }, TestName = "O First Column")]
        [TestCase(new[] { 1, 4, 7 }, TestName = "O Second Column")]
        [TestCase(new[] { 2, 5, 8 }, TestName = "O Third Column")]
        [TestCase(new[] { 0, 4, 8 }, TestName = "O First Diagonal")]
        [TestCase(new[] { 2, 4, 6 }, TestName = "O Second Diagonal")]
        public void When_BoardHasFullLineO_Then_OWins(int[] line)
        {
            var board = SetUpBoard(line, BoardMark.O);
            Assert.That(board.HasCompleteLine(BoardMark.O), Is.True);
        }
    }
}