using System;
using Tengoku.Games;
using Tengoku.Games.Spaceball;
using Tickflow;
using Trinkit;
using Trinkit.Audio;

namespace Tengoku
{
    public class GameManager : Component
    {
        public static GameManager Instance { get; private set; }

        public Conductor Conductor { get; private set; }

        public TickflowLox TickflowLox = new TickflowLox();

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
            if (Instance == null)
                Instance = this;

            TickflowLox.Run(File.ReadAllText("Resources/levels/spaceball.tkf"));
            commands.GameManager = this;

            Conductor = new Conductor();
            Conductor.Instance = Conductor;
            Conductor.InitialTempo = 104.275f;
            Conductor.Clip = Resources.Load<AudioClip>("audio/music/spaceball.wav");
            Conductor.Play();
        }

        public override void Update()
        {
            Conductor.Update();

            IsResting = !(Conductor.SongPositionInBeats >= StartRestingBeat + RestingTime);

            if (!Started)
            {
                for (int i = 0; i < TickflowLox.tokens.Count; i++)
                {
                    if (TickflowLox.tokens[i].Type == Tickflow.Tokens.TokenType.START)
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
                        var token = TickflowLox.tokens[TokenIndex];
                        TokenIndex++;

                        if (GoingToBeat && CommandBeat >= Conductor.SongPositionInBeats)
                        {
                            GoingToBeat = false;
                            return;
                        }

                        switch (token.Type)
                        {
                            case Tickflow.Tokens.TokenType.EOF:
                                commands.EOF(ref inCommandList);
                                break;
                            case Tickflow.Tokens.TokenType.NATIVE:
                                // commands.Native(TickflowLox.tokens[tokenIndex + 1].Lexeme, TickflowLox.tokens[tokenIndex + 3].Lexeme);
                                break;
                            case Tickflow.Tokens.TokenType.REST:
                                commands.Rest((double)TickflowLox.tokens[TokenIndex].Literal);
                                break;
                            case Tickflow.Tokens.TokenType.GOTO:
                                // Conductor.SetBeat((float)(double)TickflowLox.tokens[TokenIndex].Literal);
                                // goingToBeat = true;
                                break;
                            case Tickflow.Tokens.TokenType.LOG:
                                commands.Log(TickflowLox.tokens[TokenIndex].Literal);
                                break;
                            case Tickflow.Tokens.TokenType.CALL:
                                commands.Call(
                                    (string)TickflowLox.tokens[TokenIndex].Lexeme,
                                    (string)TickflowLox.tokens[TokenIndex + 2].Lexeme,
                                    TickflowLox.tokens);
                                break;
                            case Tickflow.Tokens.TokenType.SKIP:
                                SkipCommands = (int)(double)TickflowLox.tokens[TokenIndex].Literal + 1; // Add one to compensate for the skip semicolon
                                break;
                            case Tickflow.Tokens.TokenType.SEMICOLON:
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
