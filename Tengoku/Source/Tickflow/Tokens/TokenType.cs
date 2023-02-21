namespace Tickflow.Tokens
{
    public enum TokenType
    {
        // Single character tokens
        MINUS,
        PLUS,
        STAR,
        SLASH,
        LEFT_PAREN,
        RIGHT_PAREN,
        LEFT_BRACKET,
        RIGHT_BRACKET,
        COMMA,
        DOT,
        SEMICOLON,

        //One or two character tokens.
        BANG,
        BANG_EQUAL,
        EQUAL,
        EQUAL_EQUAL,
        GREATER,
        GREATER_EQUAL,
        LESS,
        LESS_EQUAL,

        // Literals
        IDENTIFIER,
        STRING,
        NUMBER,

        // Keywords
        USING,
        START,
        END,
        ENGINE,
        REST,
        CALL,
        SKIP,
        GOTO,
        NATIVE,
        IF,
        ELSE,
        TRUE,
        FALSE,
        VOID,
        FOR,
        LOG,

        EOF
    }
}
