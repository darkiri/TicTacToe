using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class BobTests
    {
        [TestCase("_XO | _OX | X__", Result = 0)]
        [TestCase("X__ | X__ | _O_", Result = 6)]
        [TestCase("XOX | X_O | _XO", Result = 6)]
        [TestCase("XOX | O_X | _XO", Result = 4)]
        [TestCase("XOX | X_O | O_O", Result = 7)]
        [TestCase("XOX | X_X | OXO", Result = 4)]
        [TestCase("XO_ | XX_ | __O", Result = 5)]
        public int BobShouldGoToTheOptimalPosition(string initialState)
        {
            var bob = new Bob();
            bob.SetMark(BoardMark.O);
            return bob.GetNextPosition(new BoardState(ParseInitialState(initialState)));
        }

        private static IEnumerable<BoardMark> ParseInitialState(string initialState) 
        {
            return initialState.Where(p => p != '|' && p != ' ').Select(p => (BoardMark)Enum.Parse(typeof (BoardMark), p.ToString()));
        }
    }
}