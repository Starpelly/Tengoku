namespace Tickscript
{
    public class GameEngine : Attribute
    {
        public GameEngine(string gameName)
        {

        }
    }

    public class GameFunction : Attribute
    {
        public enum ParamType
        {
            COMMAND_BEAT
        }

        public GameFunction(string functionName)
        {

        }

        public GameFunction(string functionName, ParamType[] types)
        {

        }
    }
}
