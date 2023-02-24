using System;
using Tengoku.Games;
using Tengoku.Games.Spaceball;
using Tickscript;
using Trinkit;
using Trinkit.Audio;

namespace Tengoku
{
    public class GameManager : Component
    {
        public static readonly GameManager Instance = new GameManager();

        public TickscriptLox TickscriptLox = new TickscriptLox();

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
            TickscriptLox.Run(File.ReadAllText("Resources/levels/spaceball.tks"));

            Conductor.Instance.InitialTempo = 104.275f;
            Conductor.Instance.Clip = Resources.Load<AudioClip>("audio/music/spaceball.wav");
            Conductor.Instance.Play();
        }

        public override void Update()
        {
            Conductor.Instance.Update();
            if (TickscriptLox == null || TickscriptLox.tokens == null) return;

            IsResting = !(Conductor.Instance.SongPositionInBeats >= StartRestingBeat + RestingTime);
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

                        if (GoingToBeat && CommandBeat >= Conductor.Instance.SongPositionInBeats)
                        {
                            GoingToBeat = false;
                            return;
                        }

                        var tokenLiteral = TickscriptLox.tokens[TokenIndex].Literal;
                        if (tokenLiteral == null) tokenLiteral = 0;

                        switch (token.Type)
                        {
                            case Tickscript.Tokens.TokenType.EOF:
                                commands.EOF(ref inCommandList);
                                break;
                            case Tickscript.Tokens.TokenType.NATIVE:
                                // commands.Native(TickscriptLox.tokens[tokenIndex + 1].Lexeme, TickscriptLox.tokens[tokenIndex + 3].Lexeme);
                                break;
                            case Tickscript.Tokens.TokenType.REST:
                                commands.Rest((double)tokenLiteral);
                                break;
                            case Tickscript.Tokens.TokenType.GOTO:
                                // Conductor.SetBeat((float)(double)TickscriptLox.tokens[TokenIndex].Literal);
                                // goingToBeat = true;
                                break;
                            case Tickscript.Tokens.TokenType.LOG:
                                commands.Log(tokenLiteral);
                                break;
                            case Tickscript.Tokens.TokenType.CALL:
                                commands.Call(
                                    (string)TickscriptLox.tokens[TokenIndex].Lexeme,
                                    (string)TickscriptLox.tokens[TokenIndex + 2].Lexeme,
                                    TickscriptLox.tokens);
                                break;
                            case Tickscript.Tokens.TokenType.SKIP:
                                SkipCommands = (int)(double)tokenLiteral + 1; // Add one to compensate for the skip semicolon
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
