using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace TicTacToe.Tests
{
    [TestFixture]
    public class PlayStrategyTests
    {
        public object[] TestData =
            {
                new object[]
                    {
                        "When a single position is free, then O should go to this position",
                        "XOX|" +
                        "X_X|" +
                        "OXO|", new[] {4}
                    },
                new object[]
                    {
                        "When X has two in a column, thwn O should block X",
                        "XOX|" +
                        "X_O|" +
                        "_XO|", new[] {6}
                    },
                new object[]
                    {
                        "When O has two in a line, then O should win",
                        "XOX|" +
                        "X_O|" +
                        "O_O|", new[] {7}
                    },
                new object[]
                    {
                        "When Center is free, then O should go to the Center",
                        "XOX|" +
                        "O_X|" +
                        "_XO|", new[] {4}
                    },
                new object[]
                    {
                        "When corner is empty, then O should go to a corner",
                        "_OX|" +
                        "_O_|" +
                        "_X_|", new[] {8, 0}
                    },
                new object[]
                    {
                        "When X has a fork, then O should at least block one of the X win possibilities",
                        "XO_|" +
                        "XX_|" +
                        "__O|", new[] {5}
                    },
                new object[]
                    {
                        "When O has possibiliy for a fork, then O should do a fork",
                        "OX_|" +
                        "_O_|" +
                        "__X|", new[] {3, 6}
                    },
                new object[]
                    {
                        "When X has possibiliy for a fork, then O should block the X's fork",
                        "_OX|" +
                        "_X_|" +
                        "O__|", new[] {5, 8}
                    },
                new object[]
                    {
                        "",
                        "_OX|" +
                        "X_O|" +
                        "___|", new[] {4, 6}
                    },
                new object[]
                    {
                        "When corner is empty, then O should go to a corner",
                        "_XO|" +
                        "_OX|" +
                        "X__|", new[] {0}
                    },
            };

        [TestCaseSource("TestData")]
        public void BobStrategy(string dummy, string initialState, params int[] expectedPositions)
        {
            AssertNextPosition(new Bob(), initialState, expectedPositions);
        }

        [TestCaseSource("TestData")]
        public void SteveStrategy(string dummy, string initialState, params int[] expectedPositions)
        {
            AssertNextPosition(new Steve(), initialState, expectedPositions);
        }

        private static void AssertNextPosition(PlayStrategy playStrategy, string initialState,
                                               IEnumerable<int> expectedPositions)
        {
            var boardState = new BoardState(ParseInitialState(initialState));
            playStrategy.SetMark(BoardMark.O);
            int nextPosition = playStrategy.GetNextPosition(boardState);
            Assert.True(expectedPositions.Any(p => p == nextPosition), nextPosition.ToString());
        }


        private static IEnumerable<BoardMark> ParseInitialState(string initialState)
        {
            return
                initialState.Where(p => p != '|' && p != ' ').Select(
                    p => (BoardMark) Enum.Parse(typeof (BoardMark), p.ToString()));
        }
    }
}