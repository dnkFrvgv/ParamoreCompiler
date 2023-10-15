using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Scanner.Enums;

namespace Scanner
{
    public class Lexer
    {
        private readonly string _sourceCode;
        private int currentPosition = 0;
        public List<Token> tokens = new List<Token>();

        public Lexer(string sourceCode)
        {
            _sourceCode = sourceCode;
        }

        public List<Token> GetTokens() { return tokens; }

        private void NextPosition() {
            currentPosition++;
        }

        public char SeeNextCharacter()
        {
            if (currentPosition + 1 >= _sourceCode.Length)
            {
                return '\0';
            }

            return _sourceCode[currentPosition + 1];
        }

        private char GetCurrentCharacter()
        {
            if (currentPosition >= _sourceCode.Length)
            {
                return '\0';
            }

            return _sourceCode[currentPosition];  
        }


        public void Tokenise()
        {

            while (GetCurrentCharacter() != '\0')
            {
                // recognising integer numbers
                if (Char.IsDigit(GetCurrentCharacter()))
                {
                    int startPosition = currentPosition;

                    while(Char.IsDigit(GetCurrentCharacter()))
                    {
                        NextPosition();
                    }
                 
                    int lengthOfNumber = currentPosition - startPosition;
                    string textNumber = _sourceCode.Substring(startPosition, lengthOfNumber);
                    try
                    {
                        int numberValue = int.Parse(textNumber);
                        tokens.Add(new Token(TokenType.NUMBER, textNumber, startPosition, numberValue));
                    }
                    catch (FormatException ex) 
                    { 
                        Console.WriteLine($"Invalid integer at {startPosition} {ex}.");
                    }

                }

                if (Char.IsWhiteSpace(GetCurrentCharacter()))
                {
                    var startPosition = currentPosition;


                }
       
                // recognising operators
                if (GetCurrentCharacter() == '+')
                {
                    tokens.Add(new Token(TokenType.PLUS, "+", currentPosition, null));
                    NextPosition();
                }
                if (GetCurrentCharacter() == '-')
                {
                    tokens.Add(new Token(TokenType.MINUS, "-", currentPosition, null));
                    currentPosition++;
                }
                if (GetCurrentCharacter() == '*')
                {
                    tokens.Add(new Token(TokenType.ASTERISK, "*", currentPosition, null));
                    currentPosition++;
                }
                if (GetCurrentCharacter() == '/')
                {
                    tokens.Add(new Token(TokenType.SLASH, "/", currentPosition, null));
                }
                if (GetCurrentCharacter() == '>')
                {
                    if(SeeNextCharacter() == '=')
                    {
                        tokens.Add(new Token(TokenType.GREATER_EQUALS, ">=", currentPosition, null));
                        NextPosition();
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.GREATER, ">", currentPosition, null));
                    }
                }
                if (GetCurrentCharacter() == '<')
                {
                    if (SeeNextCharacter() == '=')
                    {
                        tokens.Add(new Token(TokenType.LESS_EQUALS, "<=", currentPosition, null));
                        NextPosition();
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.LESS, "<", currentPosition, null));
                    }
                }

                // recognising punctuation

                if (GetCurrentCharacter() == '(') tokens.Add(new Token(TokenType.LEFT_PARENTHESES, "(", currentPosition, null));
                if (GetCurrentCharacter() == ')') tokens.Add(new Token(TokenType.RIGHT_PARENTHESES, ")", currentPosition, null));
                if (GetCurrentCharacter() == '{') tokens.Add(new Token(TokenType.LEFT_BRACES, "{", currentPosition, null));
                if (GetCurrentCharacter() == '}') tokens.Add(new Token(TokenType.RIGHT_BRACES, "}", currentPosition, null));
                if (GetCurrentCharacter() == '[') tokens.Add(new Token(TokenType.LEFT_BRACKETS, "[", currentPosition, null));
                if (GetCurrentCharacter() == ']') tokens.Add(new Token(TokenType.RIGHT_BRACKETS, "]", currentPosition, null));
                if (GetCurrentCharacter() == ',') tokens.Add(new Token(TokenType.COMMA, ",", currentPosition, null));
                if (GetCurrentCharacter() == '.') tokens.Add(new Token(TokenType.DOT, ".", currentPosition, null));
                if (GetCurrentCharacter() == ';') tokens.Add(new Token(TokenType.SEMICOLON, ";", currentPosition, null));
                if (GetCurrentCharacter() == ':') tokens.Add(new Token(TokenType.COLON, ":", currentPosition, null));

                // recognising keywords



                NextPosition();
            }
        }

    }
}
