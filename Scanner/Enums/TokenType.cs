using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scanner.Enums
{
    public enum TokenType
    {
       
        IDENTIFIER, UNKNOWN_TOKEN, END_OF_CODE,
        // Literals 
        STRING, NUMBER, WHITE_SPACE,
        // Operators
        PLUS, MINUS, ASTERISK, SLASH, EQUALS, EQUALS_EQUALS, AND, 
        OR, NOT_EQUALS, GREATER, LESS, GREATER_EQUALS, LESS_EQUALS,
        // Punctuation
        DOT, COMMA, SEMICOLON, LEFT_PARENTHESES, RIGHT_PARENTHESES,
        LEFT_BRACKETS, RIGHT_BRACKETS, LEFT_BRACES, RIGHT_BRACES, COLON,
        // Keywords
        CLASS, IF, ELSE, FOR, FALSE, TRUE, RETURN, THIS, VAR, WHILE,
        PRINT,
    }
}
