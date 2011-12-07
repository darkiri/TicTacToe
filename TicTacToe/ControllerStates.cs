using System;

namespace TicTacToe
{
    public abstract class ControllerState
    {
        public static readonly ControllerState Setup = new SetupControllerState();
        public static readonly ControllerState Play = new PlayControllerState();
        public static readonly ControllerState Quit = new EndState();
        public static readonly ControllerState GameOver = new EndState();

        public virtual ControllerState Handle(string key, IGameplay gameplay)
        {
            switch (key.ToUpper())
            {
                case "Q":
                    return Quit;
                case "R":
                    gameplay.Reset();
                    return Play;
                default:
                    return HandleOtherInput(key.ToUpper(), gameplay);
            }
        }

        public abstract string ToString(IGameplay gameplay);
        protected abstract ControllerState HandleOtherInput(string key, IGameplay gameplay);
    }

    public class EndState : ControllerState
    {
        protected override ControllerState HandleOtherInput(string key, IGameplay gameplay)
        {
            return this;
        }

        public override string ToString(IGameplay gameplay)
        {
            return "Game over. (R)epeat once again or (Q)uit?";
        }
    }

    public class PlayControllerState : ControllerState
    {
        protected override ControllerState HandleOtherInput(string key, IGameplay gameplay)
        {
            gameplay.GoTo(ParsePosition(key));
            
            return gameplay.WhoWins() != BoardMark._ 
                ? GameOver 
                : Play;
        }

        private static int ParsePosition(string key)
        {
            if (key.Length == 1 && Char.IsDigit(key[0]))
            {
                return Convert.ToInt32(key);
            }
            else
            {
                throw new FormatException("Enter position (0-8)");
            }
        }

        public override string ToString(IGameplay gameplay)
        {
            return gameplay.Board + "\n\r\n\r" + GetPlayStatus(gameplay);
        }

        public string GetPlayStatus(IGameplay gameplay)
        {
            return gameplay.WhoWins() != BoardMark._
                       ? String.Format("{0} wins!", gameplay.WhoWins())
                       : String.Format("{0} goes:", gameplay.WhoGoesNow);
        }
    }

    public class SetupControllerState : ControllerState
    {
        protected override ControllerState HandleOtherInput(string key, IGameplay gameplay)
        {
            switch (key)
            {
                case "Y":
                    gameplay.Setup(true);
                    return Play;
                case "N":
                    gameplay.Setup(false);
                    return Play;
                default:
                    return Setup;
            }
        }

        public override string ToString(IGameplay gameplay)
        {
            return "Play with the Computer? (Y/N)";
        }
    }
}