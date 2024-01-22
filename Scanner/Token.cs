using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Scanner.Enums;

namespace Scanner
{
    public class Token
    {
        public Token(TokenType type, string lexeme, int position, object literalValue)
        {
            Type = type;
            Lexeme = lexeme;
            Position = position;
            LiteralValue = literalValue;

        }
        public TokenType Type { get; }
        public int Position { get; }
        public string Lexeme { get; }
        public object LiteralValue { get; }
    }
}
