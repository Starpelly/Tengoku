using Tengoku;

namespace Tickflow
{
    public class Commands
    {
        public GameManager GameManager { get; set; }

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

        public void Call(string engine, string function, List<Tickflow.Tokens.Token> tokens)
        {
            if (IsSkipping()) return;

            List<object> parameters = new List<object>();
            int parametersIndex = 0;

            if (tokens[GameManager.TokenIndex + 3].Type == Tickflow.Tokens.TokenType.LEFT_PAREN)
            {
                GameManager.InParams = true;
            }

            while (GameManager.InParams)
            {
                var newToken = tokens[GameManager.TokenIndex + 4 + parametersIndex];
                if (newToken.Type == Tickflow.Tokens.TokenType.RIGHT_PAREN)
                {
                    GameManager.InParams = false;
                    continue;
                }
                else if (newToken.Type == Tickflow.Tokens.TokenType.COMMA)
                {
                    parametersIndex++;
                    continue;
                }

                var literal = newToken.Literal;
                if (newToken.Type == Tickflow.Tokens.TokenType.TRUE || newToken.Type == Tickflow.Tokens.TokenType.FALSE)
                    literal = (newToken.Type == Tickflow.Tokens.TokenType.TRUE) ? true : false;

                parameters.Add(literal);
                parametersIndex++;
            }

            // Debug.Log(engine + "  :  " + function);
            if (function == "ball")
                Game.Instance.spaceball.Ball(GameManager.CommandBeat, (bool)parameters[0]);
            else if (function == "zoom")
                Game.Instance.spaceball.Zoom(GameManager.CommandBeat, (float)(double)parameters[0], (float)(double)parameters[1]);
            /*else if (function == "riceball")
                GameManager.Spaceball.Riceball(GameManager.CommandBeat, (bool)parameters[0]);
            else if (function == "prepare")
                GameManager.Spaceball.DispenserPrepare();
            else if (function == "umpireShow")
                GameManager.Spaceball.Umpire(true);
            else if (function == "umpireIdle")
                GameManager.Spaceball.Umpire(false);
            else if (function == "costume")
                GameManager.Spaceball.Costume((int)(double)parameters[0], (string)parameters[1], (string)parameters[2], (string)parameters[3]);    */
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
            var methodInfo = typeInfo.GetMethod(functionName);
            methodInfo.Invoke(null, null);
        }

        public bool IsSkipping()
        {
            return GameManager.SkipCommands > 0 || GameManager.GoingToBeat;
        }
    }
}
