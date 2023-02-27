using System.Reflection;

using Tengoku.Scenes;
using Trinkit;
using Trinkit.Audio;

using Tickscript;
using System.Collections.ObjectModel;
using Tengoku.Games.Spaceball;

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

            var functionList = typeof(Spaceball).GetMethods().Where(c => c.GetCustomAttributes(typeof(GameFunction), false).Length > 0).ToList();
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

            callFunction.Invoke(game.Spaceball, callParameters.ToArray());

            /*
            if (function == "ball")
                game.Spaceball.Ball(TickManager.CommandBeat, (bool)parameters[0]);
            else if (function == "riceball")
                game.Spaceball.Ball(TickManager.CommandBeat, (bool)parameters[0], true);
            else if (function == "zoom")
                game.Spaceball.Zoom(TickManager.CommandBeat, (float)(double)parameters[0], (float)(double)parameters[1]);
            else if (function == "prepare")
                game.Spaceball.DispenserPrepare();
            else if (function == "umpireShow")
                game.Spaceball.Umpire(true);
            else if (function == "umpireIdle")
                game.Spaceball.Umpire(false);
            else if (function == "costume")
                game.Spaceball.Costume((int)(double)parameters[0], (string)parameters[1], (string)parameters[2], (string)parameters[3]);*/
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
            if (TickscriptLox == null || TickscriptLox.tokens == null) return;

            TickManager.IsResting = !(Conductor.Instance.SongPositionInBeats >= TickManager.StartRestingBeat + TickManager.RestingTime);
            if (!TickManager.Started)
            {
                for (int i = 0; i < TickscriptLox.tokens.Count; i++)
                {
                    if (TickscriptLox.tokens[i].Type == Tickscript.Tokens.TokenType.START)
                    {
                        TickManager.Started = true;
                        break;
                    }
                }
            }
            else
            {
                if (!TickManager.Ended && !TickManager.IsResting)
                {
                    bool inCommandList = true;
                    while (inCommandList || TickManager.GoingToBeat)
                    {
                        var token = TickscriptLox.tokens[TickManager.TokenIndex];
                        TickManager.IncreaseTokenIndex();

                        if (TickManager.GoingToBeat && TickManager.CommandBeat >= Conductor.Instance.SongPositionInBeats)
                        {
                            TickManager.GoingToBeat = false;
                            return;
                        }

                        switch (token.Type)
                        {
                            case Tickscript.Tokens.TokenType.EOF:
                                commands.EOF(ref inCommandList);
                                break;
                            case Tickscript.Tokens.TokenType.NATIVE:
                                // commands.Native(TickscriptLox.tokens[tokenIndex + 1].Lexeme, TickscriptLox.tokens[tokenIndex + 3].Lexeme);
                                break;
                            case Tickscript.Tokens.TokenType.REST:
                                commands.Rest((double)TickscriptLox.tokens[TickManager.TokenIndex].Literal);
                                break;
                            case Tickscript.Tokens.TokenType.GOTO:
                                // Conductor.SetBeat((float)(double)TickscriptLox.tokens[TokenIndex].Literal);
                                // goingToBeat = true;
                                break;
                            case Tickscript.Tokens.TokenType.LOG:
                                commands.Log(TickscriptLox.tokens[TickManager.TokenIndex].Literal);
                                break;
                            case Tickscript.Tokens.TokenType.CALL:
                                commands.Call(
                                    (string)TickscriptLox.tokens[TickManager.TokenIndex].Lexeme,
                                    (string)TickscriptLox.tokens[TickManager.TokenIndex + 2].Lexeme,
                                    TickscriptLox.tokens);
                                break;
                            case Tickscript.Tokens.TokenType.SKIP:
                                TickManager.SkipCommands = (int)(double)TickscriptLox.tokens[TickManager.TokenIndex].Literal + 1; // Add one to compensate for the skip semicolon
                                break;
                            case Tickscript.Tokens.TokenType.SEMICOLON:
                                TickManager.SkipCommands -= 1;
                                inCommandList = false;
                                break;
                        }
                    }
                }
            }

        }

        public override void Dispose()
        {
        }
    }
}
