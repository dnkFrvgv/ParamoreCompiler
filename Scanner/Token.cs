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
        public Token(TokenType type, string word, int position, object value)
        {
            Type = type;
            Word = word;
            Position = position;
            Value = value;

        }
        public TokenType Type { get; }
        public int Position { get; }
        public string Word { get; }
        public object Value { get; }
    }
}
