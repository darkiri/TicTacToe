﻿using System;
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
        public void when_gameNotStarted_then_theBoardIsEmpty()
        {
            var board = new Board();
            AssertBoardPositions(board.FreePositions, 0, 1, 2, 3, 4, 5, 6, 7, 8);
            AssertBoardPositions(board.XPositions);
            AssertBoardPositions(board.OPositions);
        }

        [Test]
        public void when_XGoesCorner_then_theBoardIsX00000000()
        {
            var board = new Board();
            board.SetX(0);
            AssertBoardPositions(board.FreePositions, 1, 2, 3, 4, 5, 6, 7, 8);
            AssertBoardPositions(board.XPositions, 0);
            AssertBoardPositions(board.OPositions);
        }

        [Test]
        public void when_XGoesCorner_and_OGoesSide_then_theBoardIsXO0000000()
        {
            var board = new Board();
            board.SetX(0);
            board.SetO(1);
            AssertBoardPositions(board.FreePositions, 2, 3, 4, 5, 6, 7, 8);
            AssertBoardPositions(board.XPositions, 0);
            AssertBoardPositions(board.OPositions, 1);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void when_XGoesCenter_then_OCannotGoCenter()
        {
            var board = new Board();
            board.SetX(4);
            board.SetO(4);
        }

        [Test, ExpectedException(typeof(InvalidOperationException))]
        public void when_OGoesCenter_then_XCannotGoCenter()
        {
            var board = new Board();
            board.SetO(4);
            board.SetX(4);
        }

        [Test]
        public void When_BoardResetted_Then_AllPositionsFree()
        {
            var board = new Board();
            board.SetX(0);
            board.SetO(1);
            board.Reset();
            AssertBoardPositions(board.FreePositions, 0, 1, 2, 3, 4, 5, 6, 7, 8);
        }

    }
}