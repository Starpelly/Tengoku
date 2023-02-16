using System.Collections.Generic;
using Tickflow.Tokens;

namespace Tickflow
{
    public class Scanner
    {
        public TickflowLox tickflowLox;

        private readonly string source;
        private readonly List<Token> tokens = new List<Token>();
        private int start = 0;
        private int current = 0;
        private int line = 1;
        private readonly Dictionary<string, TokenType> keywords = new Dictionary<string, TokenType>
        {
            ["using"]   = TokenType.USING,
            ["start"]   = TokenType.START,
            ["end"]     = TokenType.END,
            ["engine"]  = TokenType.ENGINE,
            ["rest"]    = TokenType.REST,
            ["call"]    = TokenType.CALL,
            ["skip"]    = TokenType.SKIP,
            ["goto"]    = TokenType.GOTO,
            ["native"]  = TokenType.NATIVE,
            ["if"]      = TokenType.IF,
            ["else"]    = TokenType.ELSE,
            ["true"]    = TokenType.TRUE,
            ["false"]   = TokenType.FALSE,
            ["void"]    = TokenType.VOID,
            ["for"]     = TokenType.FOR,
            ["log"]     = TokenType.LOG,
        };

        public Scanner(string source, TickflowLox tickflowLox)
        {
            this.source = source;
            this.tickflowLox = tickflowLox;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                start = current;
                ScanToken();
            }

            tokens.Add(new Token(TokenType.EOF, "", null, line));
            return tokens;
        }

        private void ScanToken()
        {
            var c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACKET); break;
                case '}': AddToken(TokenType.LEFT_BRACKET); break;
                case ',': AddToken(TokenType.COMMA); break;
                case '.': AddToken(TokenType.DOT); break;
                case ';': AddToken(TokenType.SEMICOLON); break;
                case '+': AddToken(TokenType.PLUS); break;
                case '-':
                    if (IsDigit(Peak()))
                        break;
                    AddToken(TokenType.MINUS); 
                    break;
                case '*': AddToken(TokenType.STAR); break;

                case '!':
                    AddToken(Match('=') ? TokenType.BANG_EQUAL : TokenType.BANG);
                    break;
                case '=':
                    AddToken(Match('=') ? TokenType.EQUAL_EQUAL : TokenType.EQUAL);
                    break;
                case '>':
                    AddToken(Match('=') ? TokenType.GREATER_EQUAL : TokenType.GREATER);
                    break;
                case '<':
                    AddToken(Match('=') ? TokenType.LESS_EQUAL : TokenType.LESS);
                    break;
                case '/': 
                    if (Match('/'))
                    {
                        while (Peak() != '\n' && !IsAtEnd()) Advance();
                    }
                    else
                    {
                        AddToken(TokenType.SLASH);
                    }
                    break;

                case ' ':
                case '\r':
                case '\t':
                    break;
                case '\n':
                    line++;
                    break;
                case '"': LiteralString(); break;

                default:
                    if (IsDigit(c))
                    {
                        LiteralNumber();
                    }
                    else if (IsAlpha(c))
                    {
                        Identifier();
                    }
                    else
                    {
                        tickflowLox.Error(line, "Unexpected character.");
                    }
                    break;
            }
        }

        /// <summary>
        /// Moves the pointer to the next lexeme and returns it.
        /// </summary>
        /// <returns></returns>
        private char Advance()
        {
            return source[current++];
        }

        /// <summary>
        /// Similar to Advance, but doesn't increment the number of analyzed characters when called.
        /// </summary>
        private char Peak()
        {
            if (IsAtEnd()) return '\0';
            return source[current];
        }

        /// <summary>
        /// Looks two characters ahead of the current character.
        /// </summary>
        private char PeakNext()
        {
            if (current + 1 >= source.Length) return '\0';
            return source[current + 1];
        }

        /// <summary>
        /// Checks if the provided lexeme is the next in the chain.
        /// If that's true, the string pointer is moved forward and we proceed.
        /// </summary>
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (source[current] != expected) return false;
            current++;
            return true;
        }


        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object literal)
        {
            var text = source.Substring(start, current - start);
            tokens.Add(new Token(type, text, literal, line));
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peak())) Advance();
            var keyword = source.Substring(start, current - start);
            TokenType type;
            var exists = keywords.TryGetValue(keyword, out type);
            if (!exists) type = TokenType.IDENTIFIER;
            AddToken(type);
        }

        /// <summary>
        /// Checks if the provided character is a valid alpha or numerical character.
        /// </summary>
        private bool IsAlphaNumeric(char v)
        {
            return IsAlpha(v) || IsDigit(v);
        }

        /// <summary>
        /// Checks if the provided character is a valid alpha character.
        /// </summary>
        private bool IsAlpha(char c)
        {
            return (c >= 'a' && c <= 'z') ||
                (c >= 'A' && c <= 'Z') ||
                c == '_';
        }

        /// <summary>
        /// Checks if the provided character is a valid numerical character.
        /// </summary>
        private bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        /// Gets a string literal.
        /// </summary>
        private void LiteralString()
        {
            while (Peak() != '"' && !IsAtEnd())
            {
                if (Peak() == '\n') line++;
                Advance();
            }

            if (IsAtEnd())
            {
                tickflowLox.Error(line, "Unterminated string.");
                return;
            }

            // Incase we find the second "
            Advance();

            var value = source.Substring(start + 1, current - start - 2);
            AddToken(TokenType.STRING, value);
        }

        private void LiteralNumber()
        {
            while (IsDigit(Peak())) Advance();

            if (Peak() == '.' && IsDigit(PeakNext()))
            {
                Advance();

                while (IsDigit(Peak())) Advance();
            }

            var negative = source[start - 1] == '-';
            var num = double.Parse(source.Substring(start, current - start));
            if (negative) num = -num;

            AddToken(TokenType.NUMBER, num);
        }

        private bool IsAtEnd()
        {
            return current >= source.Length;
        }
    }
}
