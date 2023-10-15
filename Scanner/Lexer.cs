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
        private int _currentPosition = 0;
        public List<Token> tokens = new List<Token>();

        public Lexer(string sourceCode)
        {
            _sourceCode = sourceCode;
        }

        public List<Token> GetTokens() { return tokens; }

        private void NextPosition() {
            _currentPosition++;
        }

        public char SeeNextCharacter()
        {
            if (_currentPosition + 1 >= _sourceCode.Length)
            {
                return '\0';
            }

            return _sourceCode[_currentPosition + 1];
        }

        private char GetCurrentCharacter()
        {
            if (_currentPosition >= _sourceCode.Length)
            {
                return '\0';
            }

            return _sourceCode[_currentPosition];  
        }


        public void Tokenise()
        {

            while (GetCurrentCharacter() != '\0')
            {
                // recognising integer numbers
                if (Char.IsDigit(GetCurrentCharacter()))
                {
                    int startPosition = _currentPosition;

                    while(Char.IsDigit(GetCurrentCharacter()))
                    {
                        NextPosition();
                    }
                 
                    int lengthOfNumber = _currentPosition - startPosition;
                    string textNumber = _sourceCode.Substring(startPosition, lengthOfNumber);
                    try
                    {
                        int numberValue = int.Parse(textNumber);
                        tokens.Add(new Token(TokenType.NUMBER, textNumber, startPosition, numberValue));
                        _currentPosition--;
                    }
                    catch (FormatException ex) 
                    { 
                        Console.WriteLine($"Invalid integer at {startPosition} {ex}.");
                    }

                }

                if (Char.IsWhiteSpace(GetCurrentCharacter()))
                {
                    var startPosition = _currentPosition;

                    

                    while (Char.IsWhiteSpace(GetCurrentCharacter()))
                    {
                        NextPosition();
                    }

                    var lengthOfWhiteSpace = _currentPosition - startPosition;
                    string textWhiteSpace = _sourceCode.Substring(startPosition, lengthOfWhiteSpace);
                    
                    tokens.Add(new Token(TokenType.WHITE_SPACE, textWhiteSpace, startPosition, textWhiteSpace));
                    _currentPosition--;
                   
                }
       
                // recognising operators
                if (GetCurrentCharacter() == '+') tokens.Add(new Token(TokenType.PLUS, "+", _currentPosition, null));
                
                if (GetCurrentCharacter() == '-') tokens.Add(new Token(TokenType.MINUS, "-", _currentPosition, null));
                
                if (GetCurrentCharacter() == '*') tokens.Add(new Token(TokenType.ASTERISK, "*", _currentPosition, null));
                
                if (GetCurrentCharacter() == '/') tokens.Add(new Token(TokenType.SLASH, "/", _currentPosition, null));
                if (GetCurrentCharacter() == '=')
                {
                    if (SeeNextCharacter() == '=')
                    {
                        tokens.Add(new Token(TokenType.EQUALS_EQUALS, "==", _currentPosition, null));
                        NextPosition();
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.EQUALS, "=", _currentPosition, null));
                    }
                }
                if (GetCurrentCharacter() == '>')
                {
                    if (SeeNextCharacter() == '=')
                    {
                        tokens.Add(new Token(TokenType.GREATER_EQUALS, ">=", _currentPosition, null));
                        NextPosition();
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.GREATER, ">", _currentPosition, null));
                    }
                }
                if (GetCurrentCharacter() == '<')
                {
                    if (SeeNextCharacter() == '=')
                    {
                        tokens.Add(new Token(TokenType.LESS_EQUALS, "<=", _currentPosition, null));
                        NextPosition();
                    }
                    else
                    {
                        tokens.Add(new Token(TokenType.LESS, "<", _currentPosition, null));
                    }
                }

                // recognising punctuation

                if (GetCurrentCharacter() == '(') tokens.Add(new Token(TokenType.LEFT_PARENTHESES, "(", _currentPosition, null));
                if (GetCurrentCharacter() == ')') tokens.Add(new Token(TokenType.RIGHT_PARENTHESES, ")", _currentPosition, null));
                if (GetCurrentCharacter() == '{') tokens.Add(new Token(TokenType.LEFT_BRACES, "{", _currentPosition, null));
                if (GetCurrentCharacter() == '}') tokens.Add(new Token(TokenType.RIGHT_BRACES, "}", _currentPosition, null));
                if (GetCurrentCharacter() == '[') tokens.Add(new Token(TokenType.LEFT_BRACKETS, "[", _currentPosition, null));
                if (GetCurrentCharacter() == ']') tokens.Add(new Token(TokenType.RIGHT_BRACKETS, "]", _currentPosition, null));
                if (GetCurrentCharacter() == ',') tokens.Add(new Token(TokenType.COMMA, ",", _currentPosition, null));
                if (GetCurrentCharacter() == '.') tokens.Add(new Token(TokenType.DOT, ".", _currentPosition, null));
                if (GetCurrentCharacter() == ';') tokens.Add(new Token(TokenType.SEMICOLON, ";", _currentPosition, null));
                if (GetCurrentCharacter() == ':') tokens.Add(new Token(TokenType.COLON, ":", _currentPosition, null));

                // recognising keywords



                NextPosition();
            }
        }

    }
}
