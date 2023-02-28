namespace Tickscript
{
    public class TickscriptManager
    {
        public float CommandBeat { get; internal set; }

        #region Resing

        public float RestingTime { get; internal set; }
        public float StartRestingBeat { get; internal set; }

        #endregion

        #region Looping

        public int LoopTimes { get; set; }
        public int LoopStartIndex { get; set; }
        public int LoopEndIndex { get; set; }

        #endregion

        #region Functions

        public string CurrentFunction { get; set; } = String.Empty;
        public int FunctionOpenIndex { get; set; }
        public Dictionary<string, Function> Functions = new();

        public struct Function
        {
            public int StartToken { get; set; }
            public int EndToken { get; set; }

            public Function(int startToken, int endToken)
            {
                this.StartToken = startToken;
                this.EndToken = endToken;
            }
        }

        #endregion

        public int SkipCommands { get; set; }
        public bool GoingToBeat { get; set; } = false;

        public int TokenIndex { get; internal set; }

        internal bool InParams { get; set; }

        public bool Started { get; set; }
        public bool Ended { get; internal set; }

        #region Custom

        public bool IsResting { get; set; }

        public void IncreaseTokenIndex()
        {
            TokenIndex++;
        }

        public void SetTokenIndex(int index)
        {
            TokenIndex = index;
        }

        #endregion
    }
}
