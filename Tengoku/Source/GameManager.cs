using System.Reflection;

using Tengoku.Scenes;
using Trinkit;
using Trinkit.Audio;

using Tickscript;
using Tickscript.Tokens;
using System.Collections.Specialized;

namespace Tengoku
{
    public class GameManager : Component
    {
        public TickscriptLox TickscriptLox = new TickscriptLox();

        private Commands commands = new Commands();

        public TickscriptManager TickManager => commands.Manager!;

        public GameManager()
        {
            commands.Manager = new TickscriptManager();
            LoadScript("Resources/levels/spaceball.tks");
        }

        public void LoadScript(string location)
        {
            TickscriptLox.Run(File.ReadAllText(location));
            commands.OnCommand += OnCommand;

            // This is just bad, make a proper way of doing this in the future.
            Conductor.Instance.Dispose();
            Conductor.Instance.InitialTempo = (float)(double)TickscriptLox.tokens[1].Literal;
            Conductor.Instance.Clip = Resources.Load<AudioClip>($"audio/music/{TickscriptLox.tokens[4].Literal}");
            Conductor.Instance.Play();
        }

        public void OnCommand(string engine, string function, List<object> parameters)
        {
            if (Game.Instance.CurrentScene == null) return;

            var game = (GameScene)Game.Instance.CurrentScene;
            var minigameType = game.CurrentMinigame.GetType();

            var functionList = minigameType.GetMethods().Where(c => c.GetCustomAttributes(typeof(GameFunction), false).Length > 0).ToList();
            var funcIndex = functionList.FindIndex(c => c.GetAttributeValues<GameFunction>()[0].ToString() == function);

            if (funcIndex < 0) return;

            var callFunction = functionList[funcIndex];
            var callParameters = new List<object>();

            var attributeValues = callFunction.GetAttributeValues<GameFunction>();

            for (int i = 1; i < attributeValues.Length; i++)
            {
                switch (attributeValues[i])
                {
                    case (int)GameFunction.ParamType.COMMAND_BEAT:
                        callParameters.Add(TickManager.CommandBeat);
                        break;
                }
            }

            var startCallParam = callParameters.Count;
            for (int i = 0; i < parameters.Count; i++)
            {
                var t = callFunction.GetParameters()[startCallParam + i].ParameterType;
                var nw = Convert.ChangeType(parameters[i], t);
                callParameters.Add(nw);
            }

            if (callParameters.Count < callFunction.GetParameters().Length)
            {
                for (int i = 0; i < callFunction.GetParameters().Length - callParameters.Count; i++)
                callParameters.Add(Type.Missing);
            }

            callFunction.Invoke(game.CurrentMinigame, callParameters.ToArray());
        }

        static IEnumerable<Type> GetTypesWithAttribute(Assembly assembly)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeof(GameEngine), true).Length > 0)
                {
                    yield return type;
                }
            }
        }

        public override void Update()
        {
            Conductor.Instance.Update();

            if (TickscriptLox == null || TickscriptLox.tokens == null || commands.Manager == null) return;

            TickManager.IsResting = !(Conductor.Instance.SongPositionInBeats >= TickManager.StartRestingBeat + TickManager.RestingTime);
            
            if (!TickManager.Ended && !TickManager.IsResting)
            {
                bool inCommandList = true;
                while (inCommandList || TickManager.GoingToBeat)
                {
                    var token = TickscriptLox.tokens[TickManager.TokenIndex];
                    TickManager.IncreaseTokenIndex();

                    if (TickManager.LoopTimes > 0)
                        if (TickManager.TokenIndex > TickManager.LoopEndIndex + 2)
                        {
                            TickManager.SetTokenIndex(TickManager.LoopStartIndex);
                            TickManager.LoopTimes--;
                        }

                    if (TickManager.GoingToBeat && TickManager.CommandBeat >= Conductor.Instance.SongPositionInBeats)
                    {
                        TickManager.GoingToBeat = false;
                        return;
                    }

                    SwitchToken(token, TickManager.TokenIndex, ref inCommandList);
                }
            }

        }

        private void SwitchToken(Token token, int tokenIndex, ref bool inCommandList)
        {
            if (commands.Manager == null || TickscriptLox.tokens == null) return;

            switch (token.Type)
            {
                case Tickscript.Tokens.TokenType.EOF:
                    commands.EOF(ref inCommandList);
                    break;
                case Tickscript.Tokens.TokenType.REST:
                    commands.Rest((double)TickscriptLox.tokens[tokenIndex].Literal);
                    break;
                case Tickscript.Tokens.TokenType.CALL:
                    commands.Call(
                        (string)TickscriptLox.tokens[tokenIndex].Lexeme,
                        (string)TickscriptLox.tokens[tokenIndex + 2].Lexeme,
                        TickscriptLox.tokens);
                    break;
                case Tickscript.Tokens.TokenType.SKIP:
                    TickManager.SkipCommands = (int)(double)TickscriptLox.tokens[tokenIndex].Literal + 1; // Add one to compensate for the skip semicolon
                    break;
                case Tickscript.Tokens.TokenType.SEMICOLON:
                    TickManager.SkipCommands -= 1;
                    inCommandList = false;
                    break;
                case Tickscript.Tokens.TokenType.LOOP:
                    commands.Manager.LoopTimes = (int)(double)TickscriptLox.tokens[tokenIndex + 1].Literal;
                    commands.Manager.LoopStartIndex = tokenIndex;
                    var loopBracketStart = commands.Manager.LoopStartIndex + 3;
                    var tokensPoint = TickscriptLox.tokens.GetRange(loopBracketStart, TickscriptLox.tokens.Count - loopBracketStart);
                    
                    var loopTokenCount = tokensPoint.TakeWhile(c => c.Type != TokenType.RIGHT_BRACKET).Count();
                    commands.Manager.LoopEndIndex = commands.Manager.LoopStartIndex + loopTokenCount;
                    break;
                case TokenType.FUNCTION:
                    var function = new TickscriptManager.Function();
                    function.StartToken =
                        tokenIndex + GetRangeFromIndex(tokenIndex).TakeWhile(c => c.Type != TokenType.LEFT_BRACKET).Count();

                    function.EndToken 
                        = function.StartToken + GetRangeFromIndex(function.StartToken).TakeWhile(c => c.Type != TokenType.RIGHT_BRACKET).Count();

                    commands.Manager.Functions.Add(SeekToken().Lexeme, function);

                    TickManager.SetTokenIndex(function.EndToken);
                    break;
                case TokenType.RIGHT_BRACKET:
                    if (TickManager.CurrentFunction != string.Empty)
                    {
                        if (tokenIndex - 1 == TickManager.Functions[TickManager.CurrentFunction].EndToken)
                        {
                            TickManager.CurrentFunction = string.Empty;
                            TickManager.SetTokenIndex(TickManager.FunctionOpenIndex);
                        }
                    }
                    break;
                default:
                    var lexeme = SeekToken(-1).Lexeme;
                    if (TickManager.Functions.ContainsKey(lexeme))
                    {
                        var func = TickManager.Functions[lexeme];
                        TickManager.CurrentFunction = lexeme;
                        TickManager.FunctionOpenIndex = tokenIndex;
                        TickManager.SetTokenIndex(func.StartToken);
                    }
                    break;
            }
        }

        public List<Token> GetRangeFromIndex(int startIndex)
        {
            return TickscriptLox.tokens.GetRange(startIndex, TickscriptLox.tokens.Count - startIndex);
        }

        public Token SeekToken(int seek = 0)
        {
            return TickscriptLox.tokens[TickManager.TokenIndex + seek];
        }

        public int IndexOf(OrderedDictionary dictionary, object value)
        {
            for (int i = 0; i < dictionary.Count; ++i)
            {
                if (dictionary[i] == value) return i;
            }
            return -1;
        }

        public override void Dispose()
        {
        }
    }
}
