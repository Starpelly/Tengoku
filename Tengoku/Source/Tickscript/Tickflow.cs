using Tickscript.Tokens;

namespace Tickscript
{
    public class TickscriptLox
    {
        // private bool HadErrors = false;
        public Scanner? scanner;
        public List<Token>? tokens;

        public void RunFile(string path)
        {
            var source = File.ReadAllText(path);
            Run(source);
        }

        public void Run(string source)
        {
            scanner = new Scanner(source, this);
            tokens = scanner.ScanTokens();

            foreach (var token in tokens)
            {
                // Debug.Log(token);
            }
        }

        /// <summary>
        /// Provides error information to the Report method.
        /// </summary>
        public void Error(int line, string message)
        {
            Report(line, "", message);
        }

        /// <summary>
        /// Writes console error messages.
        /// </summary>
        public void Report(int line, string where, string message)
        {
            Console.WriteLine($"[line {line}] Error {where}: {message}");
            // HadErrors = true;
        }
    }
}
