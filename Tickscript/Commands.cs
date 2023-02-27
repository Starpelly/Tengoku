namespace Tickscript
{
    public class Commands
    {
        public delegate void CommandEvent(string engine, string function, List<object> parameters);
        public event CommandEvent? OnCommand;

        public TickscriptManager? Manager { get; set; }

        public void Log(object val)
        {
            if (IsSkipping()) return;

            // Debug.Log(val);
        }

        public void Rest(double time)
        {
            if (Manager == null) return;
            // if (IsSkipping()) GameManager.commandBeat += GameManager.restingTime;
            if (IsSkipping()) return;

            Manager.RestingTime = (float)time;
            Manager.StartRestingBeat = Manager.CommandBeat;
            Manager.CommandBeat += Manager.RestingTime;
        }

        public void Call(string engine, string function, List<Tickscript.Tokens.Token> tokens)
        {
            if (Manager == null) return;
            if (IsSkipping()) return;

            List<object> parameters = new List<object>();
            int parametersIndex = 0;

            if (tokens[Manager.TokenIndex + 3].Type == Tickscript.Tokens.TokenType.LEFT_PAREN)
            {
                Manager.InParams = true;
            }

            while (Manager.InParams)
            {
                var newToken = tokens[Manager.TokenIndex + 4 + parametersIndex];
                if (newToken.Type == Tickscript.Tokens.TokenType.RIGHT_PAREN)
                {
                    Manager.InParams = false;
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
        }

        public void EOF(ref bool inCommandList)
        {
            if (Manager == null) return;

            Manager.Ended = true;
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
            if (Manager == null) return false;
            return Manager.SkipCommands > 0 || Manager.GoingToBeat;
        }
    }
}
