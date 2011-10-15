using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class SymmetryTests
    {
        [Test]
        public void When_BoardHasXInCorner_Then_ThereAre5FreeUniquePositions()
        {
            var board = new BoardState().Set(0, BoardMark.X);
            AssertUniquePositions(board, new[] { 1, 2, 4, 5, 8 });
        }

        private static void AssertUniquePositions(BoardState board, int[] expected)
        {
            Assert.That(new BoardSymmetry(board).GetFreeUniquePositions(), Is.EquivalentTo(expected));
        }

        [Test]
        public void When_BoardHasXAndOInFirstLine_Then_ThereAre7FreeUniquePositions()
        {
            var board = new BoardState().Set(0, BoardMark.X).Set(1, BoardMark.O);
            AssertUniquePositions(board, new[] { 2, 3, 4, 5, 6, 7, 8 });
        }

        [Test]
        public void When_BoardHasXInRightCorner_Then_ThereAre5FreeUniquePositions()
        {
            var board = new BoardState().Set(2, BoardMark.X);
            AssertUniquePositions(board, new[] { 0, 1, 3, 4, 6 });
        }

        [Test]
        public void When_BoardHasXInTheMiddleLeft_Then_ThereAre5FreeUniquePositions()
        {
            var board = new BoardState().Set(3, BoardMark.X);
            AssertUniquePositions(board, new[] { 0, 1, 2, 4, 5 });
        }

        [Test]
        public void When_BoardHasXInTheMiddle_Then_ThereAre5FreeUniquePositions()
        {
            var board = new BoardState().Set(1, BoardMark.X);
            AssertUniquePositions(board, new[] { 0, 3, 4, 6, 7 });
        }

        [Test]
        public void When_BoardIsEmpty_Then_ThereAre3FreeUniquePositions()
        {
            AssertUniquePositions(new BoardState(), new[] { 0, 1, 4 });
        }

        [Test]
        public void When_BoardHasXAndOInFirstColumn_Then_ThereAre7FreeUniquePositions()
        {
            var board = new BoardState().Set(0, BoardMark.X).Set(3, BoardMark.O);
            AssertUniquePositions(board, new[] { 1, 2, 4, 5, 6, 7, 8 });
        }

        [Test]
        public void When_BoardHasFullMiddle_Then_ThereAre3FreeUniquePositions()
        {
            var board = new BoardState().Set(3, BoardMark.X).Set(4, BoardMark.O).Set(5, BoardMark.O);
            AssertUniquePositions(board, new[] { 0, 1, 2 });
        }
    }
}