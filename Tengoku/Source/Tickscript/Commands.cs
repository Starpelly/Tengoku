using Tengoku;

namespace Tickscript
{
    public class Commands
    {
        public delegate void CommandEvent(string engine, string function, List<object> parameters);
        public event CommandEvent? OnCommand;

        public GameManager? gameManager { get; set; }

        public void Log(object val)
        {
            if (IsSkipping()) return;

            // Debug.Log(val);
        }

        public void Rest(double time)
        {
            // if (IsSkipping()) GameManager.commandBeat += GameManager.restingTime;
            if (gameManager == null) return;
            if (IsSkipping()) return;

            gameManager.RestingTime = (float)time;
            gameManager.StartRestingBeat = gameManager.CommandBeat;
            gameManager.CommandBeat += gameManager.RestingTime;
        }

        public void Call(string engine, string function, List<Tickscript.Tokens.Token> tokens)
        {
            if (gameManager == null) return;
            if (IsSkipping()) return;

            List<object> parameters = new List<object>();
            int parametersIndex = 0;

            if (tokens[gameManager.TokenIndex + 3].Type == Tickscript.Tokens.TokenType.LEFT_PAREN)
            {
                gameManager.InParams = true;
            }

            while (gameManager.InParams)
            {
                var newToken = tokens[gameManager.TokenIndex + 4 + parametersIndex];
                if (newToken.Type == Tickscript.Tokens.TokenType.RIGHT_PAREN)
                {
                    gameManager.InParams = false;
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

            OnCommand!.Invoke(engine, function, parameters);

            // Debug.Log(engine + "  :  " + function);
            /**/
        }

        public void EOF(ref bool inCommandList)
        {
            if (gameManager == null) return;
            gameManager.Ended = true;
            inCommandList = false;
        }

        public void Native(string fullInvokeName)
        {
            if (gameManager == null) return;
            if (IsSkipping()) return;

            var className = fullInvokeName;
            var functionName = fullInvokeName;

            var typeInfo = Type.GetType(className);
            var methodInfo = typeInfo?.GetMethod(functionName);
            methodInfo?.Invoke(null, null);
        }

        public bool IsSkipping()
        {
            if (gameManager == null) return true;

            return gameManager.SkipCommands > 0 || gameManager.GoingToBeat;
        }
    }
}
