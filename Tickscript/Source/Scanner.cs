using Tickscript.Tokens;

namespace Tickscript
{
    public class Scanner
    {
        public TickscriptLox TickscriptLox;

        private readonly string _source;
        private readonly List<Token> _tokens = new List<Token>();
        private int _start = 0;
        private int _current = 0;
        private int _line = 1;
        private readonly Dictionary<string, TokenType> _keywords = new Dictionary<string, TokenType>
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
            ["loop"]     = TokenType.LOOP,
            ["fun"]    = TokenType.FUNCTION,
        };

        public Scanner(string _source, TickscriptLox TickscriptLox)
        {
            this._source = _source;
            this.TickscriptLox = TickscriptLox;
        }

        public List<Token> ScanTokens()
        {
            while (!IsAtEnd())
            {
                _start = _current;
                ScanToken();
            }

            _tokens.Add(new Token(TokenType.EOF, "", null, _line));
            return _tokens;
        }

        private void ScanToken()
        {
            var c = Advance();
            switch (c)
            {
                case '(': AddToken(TokenType.LEFT_PAREN); break;
                case ')': AddToken(TokenType.RIGHT_PAREN); break;
                case '{': AddToken(TokenType.LEFT_BRACKET); break;
                case '}': AddToken(TokenType.RIGHT_BRACKET); break;
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
                    _line++;
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
                        TickscriptLox.Error(_line, "Unexpected character.");
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
            return _source[_current++];
        }

        /// <summary>
        /// Similar to Advance, but doesn't increment the number of analyzed characters when called.
        /// </summary>
        private char Peak()
        {
            if (IsAtEnd()) return '\0';
            return _source[_current];
        }

        /// <summary>
        /// Looks two characters ahead of the _current character.
        /// </summary>
        private char PeakNext()
        {
            if (_current + 1 >= _source.Length) return '\0';
            return _source[_current + 1];
        }

        /// <summary>
        /// Checks if the provided lexeme is the next in the chain.
        /// If that's true, the string pointer is moved forward and we proceed.
        /// </summary>
        private bool Match(char expected)
        {
            if (IsAtEnd()) return false;
            if (_source[_current] != expected) return false;
            _current++;
            return true;
        }


        private void AddToken(TokenType type)
        {
            AddToken(type, null);
        }

        private void AddToken(TokenType type, object? literal)
        {
            var text = _source.Substring(_start, _current - _start);
            _tokens.Add(new Token(type, text, literal, _line));
        }

        private void Identifier()
        {
            while (IsAlphaNumeric(Peak())) Advance();
            var keyword = _source.Substring(_start, _current - _start);
            TokenType type;
            var exists = _keywords.TryGetValue(keyword, out type);
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
                if (Peak() == '\n') _line++;
                Advance();
            }

            if (IsAtEnd())
            {
                TickscriptLox.Error(_line, "Unterminated string.");
                return;
            }

            // Incase we find the second "
            Advance();

            var value = _source.Substring(_start + 1, _current - _start - 2);
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

            var negative = _source[_start - 1] == '-';
            var num = double.Parse(_source.Substring(_start, _current - _start));
            if (negative) num = -num;

            AddToken(TokenType.NUMBER, num);
        }

        private bool IsAtEnd()
        {
            return _current >= _source.Length;
        }
    }
}
