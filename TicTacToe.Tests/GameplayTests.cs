using System;
using System.Linq;
using Machine.Specifications;
using Moq;
using NUnit.Framework;
using It = Machine.Specifications.It;

namespace TicTacToe.Tests
{
    public class with_gameplay
    {
        public static IGameplay Gameplay;
        public static Mock<PlayStrategy> Ai;
        public static int GameplayChanged;

        Establish context = () =>
        {
            Ai = new Mock<PlayStrategy>();
            Ai.Setup(p => p.GetNextPosition(Moq.It.IsAny<BoardState>())).Returns(1);
            Gameplay = new Gameplay(Ai.Object);
            GameplayChanged = 0;
            Gameplay.Changed += (_, __) => GameplayChanged++;           
        };
    }

    [Subject(typeof(Gameplay))]
    public class when_X_goes : with_gameplay
    {
        Because of = () => Gameplay.GoTo(1);
        It should_be_set_on_the_board = () => Assert.That(Gameplay.Board.GetPositions(BoardMark.X), Is.EquivalentTo(new[] { 1 }));
        It should_trigger_change_event = () => Assert.That(GameplayChanged, Is.EqualTo(1));
        It cannot_go_same_position_again = () => Catch.Exception(() => Gameplay.GoTo(1)).ShouldBeOfType<InvalidOperationException>();
        It should_not_win = () => Assert.That(Gameplay.WhoWins(), Is.EqualTo(BoardMark._));
        It should_switch_to_O = () => Assert.That(Gameplay.WhoGoesNow, Is.EqualTo(BoardMark.O));
    }

    [Subject(typeof(Gameplay))]
    public class when_resetted : with_gameplay
    {
        Because of = () => Gameplay.Reset();
        It should_trigger_change_event = () => Assert.That(GameplayChanged, Is.EqualTo(1));
        It should_reset_board = () => Assert.That(Gameplay.Board.FreePositions.Count(), Is.EqualTo(9));
        It should_set_next_turn_to_X = () => Assert.That(Gameplay.WhoGoesNow, Is.EqualTo(BoardMark.X));
        It should_not_win = () => Assert.That(Gameplay.WhoWins(), Is.EqualTo(BoardMark._));
    }

    [Subject(typeof(Gameplay))]
    public class when_setted_up : with_gameplay
    {
        Because of = () => Gameplay.Setup(true);
        It should_trigger_change_event = () => Assert.That(GameplayChanged, Is.EqualTo(1));
        It should_not_win = () => Assert.That(Gameplay.WhoWins(), Is.EqualTo(BoardMark._));
    }

    [Subject(typeof(Gameplay))]
    public class when_playing_with_computer : with_gameplay
    {
        Because of = () =>
        {
            Gameplay.Setup(true);
            GameplayChanged = 0;
            Gameplay.GoTo(0);
        };
        It should_trigger_change_event_twice = () => Assert.That(GameplayChanged, Is.EqualTo(2));
        It should_automatically_calculate_position = () => Ai.Verify(ai => ai.GetNextPosition(Moq.It.IsAny<BoardState>()));
    }

    [Subject(typeof(Gameplay))]
    public class when_playing_without_computer : with_gameplay
    {
        Because of = () =>
        {
            Gameplay.Setup(false);
            GameplayChanged = 0;
            Gameplay.GoTo(0);
        };
        It should_trigger_change_event = () => Assert.That(GameplayChanged, Is.EqualTo(1));
        It should_not_automatically_calculate_position = () => Ai.Verify(ai => ai.GetNextPosition(Gameplay.Board), Times.Never());
    }
}