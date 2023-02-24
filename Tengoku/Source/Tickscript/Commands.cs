using Tengoku;

namespace Tickscript
{
    public class Commands
    {
        public GameManager GameManager => GameManager.Instance;

        public void Log(object val)
        {
            if (IsSkipping()) return;

            // Debug.Log(val);
        }

        public void Rest(double time)
        {
            // if (IsSkipping()) GameManager.commandBeat += GameManager.restingTime;
            if (IsSkipping()) return;

            GameManager.RestingTime = (float)time;
            GameManager.StartRestingBeat = GameManager.CommandBeat;
            GameManager.CommandBeat += GameManager.RestingTime;
        }

        public void Call(string engine, string function, List<Tickscript.Tokens.Token> tokens)
        {
            if (IsSkipping()) return;

            List<object> parameters = new List<object>();
            int parametersIndex = 0;

            if (tokens[GameManager.TokenIndex + 3].Type == Tickscript.Tokens.TokenType.LEFT_PAREN)
            {
                GameManager.InParams = true;
            }

            while (GameManager.InParams)
            {
                var newToken = tokens[GameManager.TokenIndex + 4 + parametersIndex];
                if (newToken.Type == Tickscript.Tokens.TokenType.RIGHT_PAREN)
                {
                    GameManager.InParams = false;
                    continue;
                }
                else if (newToken.Type == Tickscript.Tokens.TokenType.COMMA)
                {
                    parametersIndex++;
                    continue;
                }

                var literal = newToken.Literal;
                if (newToken.Type == Tickscript.Tokens.TokenType.TRUE || newToken.Type == Tickscript.Tokens.TokenType.FALSE)
                    literal = (newToken.Type == Tickscript.Tokens.TokenType.TRUE) ? true : false;

                if (literal != null)
                    parameters.Add(literal);
                parametersIndex++;
            }

            // Debug.Log(engine + "  :  " + function);
            if (function == "ball")
                Game.Instance.spaceball.Ball(GameManager.CommandBeat, (bool)parameters[0]);
            else if (function == "riceball")
                Game.Instance.spaceball.Ball(GameManager.CommandBeat, (bool)parameters[0], true);
            else if (function == "zoom")
                Game.Instance.spaceball.Zoom(GameManager.CommandBeat, (float)(double)parameters[0], (float)(double)parameters[1]);
            else if (function == "prepare")
                Game.Instance.spaceball.DispenserPrepare();
            else if (function == "umpireShow")
                Game.Instance.spaceball.Umpire(true);
            else if (function == "umpireIdle")
                Game.Instance.spaceball.Umpire(false);
            else if (function == "costume")
                Game.Instance.spaceball.Costume((int)(double)parameters[0], (string)parameters[1], (string)parameters[2], (string)parameters[3]);
        }

        public void EOF(ref bool inCommandList)
        {
            GameManager.Ended = true;
            inCommandList = false;
        }

        public void Native(string fullInvokeName)
        {
            if (IsSkipping()) return;

            var className = fullInvokeName;
            var functionName = fullInvokeName;

            var typeInfo = Type.GetType(className);
            var methodInfo = typeInfo?.GetMethod(functionName);
            methodInfo?.Invoke(null, null);
        }

        public bool IsSkipping()
        {
            return GameManager.SkipCommands > 0 || GameManager.GoingToBeat;
        }
    }
}
