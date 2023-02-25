using Tengoku.Scenes;
using Tickscript;
using Trinkit;
using Trinkit.Audio;

namespace Tengoku
{
    public class GameManager : Component
    {
        public static GameManager Instance { get; private set; }

        public TickscriptLox TickscriptLox = new TickscriptLox();
        public Conductor Conductor = new Conductor();

        private Commands commands = new Commands();

        public bool Started = false;
        public bool Ended = false;
        public float CommandBeat;
        public int TokenIndex;

        public bool IsResting = false;
        public float RestingTime;
        public float StartRestingBeat;
        public int SkipCommands;
        public bool GoingToBeat = false;
        public bool InParams;

        public GameManager()
        {
            Instance = this;

            commands.gameManager = this;
            LoadScript("Resources/levels/spaceball.tks");
            Conductor = new Conductor();
        }

        public void LoadScript(string location)
        {
            TickscriptLox.Run(File.ReadAllText(location));
            commands.OnCommand += OnCommand;

            // This is just bad, make a proper way of doing this in the future.
            Conductor.Dispose();
            Conductor.InitialTempo = (float)(double)TickscriptLox.tokens[1].Literal;
            Conductor.Clip = Resources.Load<AudioClip>($"audio/music/{TickscriptLox.tokens[4].Literal}");
            Conductor.Play();
        }

        public void OnCommand(string engine, string function, List<object> parameters)
        {
            var game = (GameScene)Game.Instance.scene;
            if (function == "ball")
                game.Spaceball.Ball(CommandBeat, (bool)parameters[0]);
            else if (function == "riceball")
                game.Spaceball.Ball(CommandBeat, (bool)parameters[0], true);
            else if (function == "zoom")
                game.Spaceball.Zoom(CommandBeat, (float)(double)parameters[0], (float)(double)parameters[1]);
            else if (function == "prepare")
                game.Spaceball.DispenserPrepare();
            else if (function == "umpireShow")
                game.Spaceball.Umpire(true);
            else if (function == "umpireIdle")
                game.Spaceball.Umpire(false);
            else if (function == "costume")
                game.Spaceball.Costume((int)(double)parameters[0], (string)parameters[1], (string)parameters[2], (string)parameters[3]);
        }

        public override void Update()
        {
            Conductor.Update();
            if (TickscriptLox == null || TickscriptLox.tokens == null) return;

            IsResting = !(Conductor.SongPositionInBeats >= StartRestingBeat + RestingTime);
            if (!Started)
            {
                for (int i = 0; i < TickscriptLox.tokens.Count; i++)
                {
                    if (TickscriptLox.tokens[i].Type == Tickscript.Tokens.TokenType.START)
                    {
                        Started = true;
                        break;
                    }
                }
            }
            else
            {
                if (!Ended && !IsResting)
                {
                    bool inCommandList = true;
                    while (inCommandList || GoingToBeat)
                    {
                        var token = TickscriptLox.tokens[TokenIndex];
                        TokenIndex++;

                        if (GoingToBeat && CommandBeat >= Conductor.SongPositionInBeats)
                        {
                            GoingToBeat = false;
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
                                commands.Rest((double)TickscriptLox.tokens[TokenIndex].Literal);
                                break;
                            case Tickscript.Tokens.TokenType.GOTO:
                                // Conductor.SetBeat((float)(double)TickscriptLox.tokens[TokenIndex].Literal);
                                // goingToBeat = true;
                                break;
                            case Tickscript.Tokens.TokenType.LOG:
                                commands.Log(TickscriptLox.tokens[TokenIndex].Literal);
                                break;
                            case Tickscript.Tokens.TokenType.CALL:
                                commands.Call(
                                    (string)TickscriptLox.tokens[TokenIndex].Lexeme,
                                    (string)TickscriptLox.tokens[TokenIndex + 2].Lexeme,
                                    TickscriptLox.tokens);
                                break;
                            case Tickscript.Tokens.TokenType.SKIP:
                                SkipCommands = (int)(double)TickscriptLox.tokens[TokenIndex].Literal + 1; // Add one to compensate for the skip semicolon
                                break;
                            case Tickscript.Tokens.TokenType.SEMICOLON:
                                SkipCommands -= 1;
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
