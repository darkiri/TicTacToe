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
            var board = new BoardState();
            AssertBoardPositions(board.FreePositions, 0, 1, 2, 3, 4, 5, 6, 7, 8);
            AssertBoardPositions(board.GetPositions(BoardMark.X));
            AssertBoardPositions(board.GetPositions(BoardMark.O));
        }

        [Test]
        public void When_XGoesCorner_Then_TheBoardIsX00000000()
        {
            var board = new BoardState().Set(0, BoardMark.X);
            AssertBoardPositions(board.FreePositions, 1, 2, 3, 4, 5, 6, 7, 8);
            AssertBoardPositions(board.GetPositions(BoardMark.X), 0);
            AssertBoardPositions(board.GetPositions(BoardMark.O));
        }

        [Test]
        public void When_XGoesCorner_and_OGoesSide_Then_TheBoardIsXO0000000()
        {
            var board = new BoardState().Set(0, BoardMark.X).Set(1, BoardMark.O);
            AssertBoardPositions(board.FreePositions, 2, 3, 4, 5, 6, 7, 8);
            AssertBoardPositions(board.GetPositions(BoardMark.X), 0);
            AssertBoardPositions(board.GetPositions(BoardMark.O), 1);
        }

        [Test, ExpectedException(typeof (InvalidOperationException))]
        public void When_XGoesCenter_Then_OCannotGoCenter()
        {
            new BoardState().Set(4, BoardMark.X).Set(4, BoardMark.O);
        }

        [TestCase(new[] {0, 1, 2}, TestName = "X First Line")]
        [TestCase(new[] {3, 4, 5}, TestName = "X Second Line")]
        [TestCase(new[] {6, 7, 8}, TestName = "X Third Line")]
        [TestCase(new[] {0, 3, 6}, TestName = "X First Column")]
        [TestCase(new[] {1, 4, 7}, TestName = "X Second Column")]
        [TestCase(new[] {2, 5, 8}, TestName = "X Third Column")]
        [TestCase(new[] {0, 4, 8}, TestName = "X First Diagonal")]
        [TestCase(new[] {2, 4, 6}, TestName = "X Second Diagonal")]
        public void When_BoardHasFullLineX_Then_XWins(int[] line)
        {
            var board = SetUpBoard(line, BoardMark.X);
            Assert.That(board.HasCompleteLine(BoardMark.X), Is.True);
        }

        private static BoardState SetUpBoard(IEnumerable<int> line, BoardMark mark)
        {
            return line.Aggregate(new BoardState(), (current, position) => current.Set(position, mark));
        }

        [TestCase(new[] {0, 1, 2}, TestName = "O First Line")]
        [TestCase(new[] {3, 4, 5}, TestName = "O Second Line")]
        [TestCase(new[] {6, 7, 8}, TestName = "O Third Line")]
        [TestCase(new[] {0, 3, 6}, TestName = "O First Column")]
        [TestCase(new[] {1, 4, 7}, TestName = "O Second Column")]
        [TestCase(new[] {2, 5, 8}, TestName = "O Third Column")]
        [TestCase(new[] {0, 4, 8}, TestName = "O First Diagonal")]
        [TestCase(new[] {2, 4, 6}, TestName = "O Second Diagonal")]
        public void When_BoardHasFullLineO_Then_OWins(int[] line)
        {
            var board = SetUpBoard(line, BoardMark.O);
            Assert.That(board.HasCompleteLine(BoardMark.O), Is.True);
        }

        [Test]
        public void When_BoardHasXInCorner_Then_ThereAre5FreeUniquePositions()
        {
            var board = new BoardState().Set(0, BoardMark.X);
            Assert.That(board.GetFreeUniquePositions(), Is.EquivalentTo(new[] {1, 2, 4, 5, 8}));
        }

        [Test]
        public void When_BoardHasXAndOInFirstLine_Then_ThereAre7FreeUniquePositions()
        {
            var board = new BoardState().Set(0, BoardMark.X).Set(1, BoardMark.O);
            Assert.That(board.GetFreeUniquePositions(), Is.EquivalentTo(new[] {2, 3, 4, 5, 6, 7, 8}));
        }

        [Test]
        public void When_BoardHasXInRightCorner_Then_ThereAre5FreeUniquePositions()
        {
            var board = new BoardState().Set(2, BoardMark.X);
            Assert.That(board.GetFreeUniquePositions(), Is.EquivalentTo(new[] {0, 1, 3, 4, 6}));
        }

        [Test]
        public void When_BoardHasXInTheMiddleLeft_Then_ThereAre5FreeUniquePositions()
        {
            var board = new BoardState().Set(3, BoardMark.X);
            Assert.That(board.GetFreeUniquePositions(), Is.EquivalentTo(new[] {0, 1, 2, 4, 5}));
        }

        [Test]
        public void When_BoardHasXInTheMiddle_Then_ThereAre5FreeUniquePositions()
        {
            var board = new BoardState().Set(1, BoardMark.X);
            Assert.That(board.GetFreeUniquePositions(), Is.EquivalentTo(new[] {0, 3, 4, 6, 7}));
        }

        [Test]
        public void When_BoardIsEmpty_Then_ThereAre3FreeUniquePositions()
        {
            Assert.That(new BoardState().GetFreeUniquePositions(), Is.EquivalentTo(new[] {0, 1, 4}));
        }

        [Test]
        public void When_BoardHasXAndOInFirstColumn_Then_ThereAre7FreeUniquePositions()
        {
            var board = new BoardState().Set(0, BoardMark.X).Set(3, BoardMark.O);
            Assert.That(board.GetFreeUniquePositions(), Is.EquivalentTo(new[] {1, 2, 4, 5, 6, 7, 8}));
        }

        [Test]
        public void When_BoardHasFullMiddle_Then_ThereAre3FreeUniquePositions()
        {
            var board = new BoardState().Set(3, BoardMark.X).Set(4, BoardMark.O).Set(5, BoardMark.O);
            Assert.That(board.GetFreeUniquePositions(), Is.EquivalentTo(new[] {0, 1, 2}));
        }
    }
}